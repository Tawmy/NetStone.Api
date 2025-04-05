using System.Data;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetStone.Cache;
using NetStone.Cache.Db;
using NetStone.Cache.Interfaces;
using NetStone.Common.Extensions;
using NetStone.Data.Interfaces;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NetStone.Api.Extensions;

internal static class StartupExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(x =>
            {
                x.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // read version from request headers
                x.DefaultApiVersion = new ApiVersion(2);
                x.AssumeDefaultVersionWhenUnspecified = true; // assume V2 if request is sent without version
                x.ReportApiVersions = true; // respond with supported versions in response header
            })
            .AddMvc()
            .AddApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{typeof(Program).Assembly.GetName().Name}.xml"));
            options.CustomSchemaIds(type => type.ToString());
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void AddAuthentication(this WebApplicationBuilder webAppBuilder)
    {
        webAppBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority =
                    webAppBuilder.Configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAuthority);
                options.Audience =
                    webAppBuilder.Configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAudience);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateTokenReplay = true
                };
            });
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        await dbContext.Database.MigrateAsync();

        if (dbContext.Database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
        {
            if (npgsqlConnection.State != ConnectionState.Open)
            {
                await npgsqlConnection.OpenAsync();
            }

            try
            {
                await npgsqlConnection.ReloadTypesAsync();
            }
            finally
            {
                await npgsqlConnection.CloseAsync();
            }
        }
    }

    public static bool AddOtelMetrics(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (configuration.GetOptionalConfiguration<bool>(EnvironmentVariables.MetricsEnabled) is not true)
        {
            return false;
        }

        builder.Services.AddOpenTelemetry().WithMetrics(x => x.AddAspNetCoreInstrumentation()
            .AddPrometheusExporter());

        return true;
    }

    public static bool AddOtelTracing(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (configuration[EnvironmentVariables.OtelEndpointUri] is not { } otelUri)
        {
            return false;
        }

        var serviceName = Assembly.GetCallingAssembly().GetName().Name ??
                          throw new InvalidOperationException("Service name must not be null");

        builder.Services.AddOpenTelemetry().WithTracing(x =>
        {
            x.AddAspNetCoreInstrumentation(y => y.RecordException = true);
            x.AddHttpClientInstrumentation(y => y.RecordException = true);
            x.AddEntityFrameworkCoreInstrumentation(y => y.SetDbStatementForText = true);
            x.AddSource(nameof(IAutoMapperService));
            x.AddSource(nameof(INetStoneService));
            x.AddSource(nameof(ICharacterCachingServiceV3));
            x.AddSource(nameof(ICharacterCachingServiceV2));
            x.AddSource(nameof(ICharacterServiceV3));
            x.AddSource(nameof(ICharacterServiceV2));
            x.AddSource(nameof(IFreeCompanyCachingServiceV2));
            x.AddSource(nameof(IFreeCompanyServiceV3));
            x.AddSource(nameof(IFreeCompanyServiceV2));
            x.AddSource(GetMappingExtensionsClassNames());
            x.AddOtlpExporter(y => y.Endpoint = new Uri(otelUri));
        }).ConfigureResource(x => x.AddService(serviceName));

        builder.Logging.AddOpenTelemetry(x =>
        {
            x.AttachLogsToActivityEvent();
            x.IncludeFormattedMessage = true;
        });

        return true;
    }

    public static void AddDataProtection(this IServiceCollection services, IConfiguration configuration)
    {
        var certificatePath = configuration.GetGuardedConfiguration(EnvironmentVariables.DataProtectionCertificate);
        var certificate = X509Certificate2.CreateFromPemFile($"{certificatePath}.pem", $"{certificatePath}.key");

        X509Certificate2[] decryptionCertificates;
        if (configuration[EnvironmentVariables.DataProtectionCertificateAlt] is { } certificateAltPath)
        {
            // alternative certificate for decryption provided, use both
            var certificateAlt = X509Certificate2.CreateFromPemFile(
                $"{certificateAltPath}.pem", $"{certificateAltPath}.key");
            decryptionCertificates = [certificate, certificateAlt];
        }
        else
        {
            // only one certificate provided
            decryptionCertificates = [certificate];
        }

        services.AddDataProtection()
            .PersistKeysToDbContext<DatabaseContext>()
            .ProtectKeysWithCertificate(certificate)
            .UnprotectKeysWithAnyCertificate(decryptionCertificates.ToArray());
    }

    private static string[] GetMappingExtensionsClassNames()
    {
        return Assembly
            .GetAssembly(typeof(DependencyInjection))?
            .GetTypes()
            .Where(x => x.Namespace is not null &&
                        x.Namespace.StartsWith("NetStone.Cache.Extensions.Mapping") &&
                        x.ReflectedType is null)
            .Select(x => x.Name)
            .ToArray() ?? [];
    }

    public static void UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health").RequireAuthorization();

        app.MapHealthChecks("/health/db", new HealthCheckOptions
                { Predicate = check => check.Name.Equals(nameof(DatabaseContext), StringComparison.OrdinalIgnoreCase) })
            .RequireAuthorization();

        app.MapHealthChecks("/health/cert", new HealthCheckOptions
                { Predicate = check => check.Name.Equals("cert", StringComparison.OrdinalIgnoreCase) })
            .RequireAuthorization();
    }
}