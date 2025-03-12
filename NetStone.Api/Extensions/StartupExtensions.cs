using System.Data;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetStone.Cache.Db;
using NetStone.Common.Extensions;
using Npgsql;

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
}