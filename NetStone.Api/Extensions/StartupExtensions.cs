using System.Data;
using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            x.AddSource(nameof(ICharacterCachingService));
            x.AddSource(nameof(ICharacterService));
            x.AddSource(nameof(IFreeCompanyCachingService));
            x.AddSource(nameof(IFreeCompanyService));
            x.AddSource(nameof(IFreeCompanyService));
            x.AddOtlpExporter(y => y.Endpoint = new Uri(otelUri));
        }).ConfigureResource(x => x.AddService(serviceName));

        builder.Logging.AddOpenTelemetry(x =>
        {
            x.AttachLogsToActivityEvent();
            x.IncludeFormattedMessage = true;
        });

        return true;
    }
}