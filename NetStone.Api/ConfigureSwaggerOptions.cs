using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetStone.Common.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetStone.Api;

internal class ConfigureSwaggerOptions(
    IConfiguration configuration,
    IApiVersionDescriptionProvider provider,
    Version version)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    /// <summary>
    ///     Configure each API discovered for Swagger Documentation
    /// </summary>
    public void Configure(SwaggerGenOptions options)
    {
        // Get environment variables for Swagger auth
        var authority = configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAuthority);

        var tokenUrl = Path.Combine(authority, "protocol/openid-connect/token");

        // Add Keycloak auth to Swagger UI
        options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow
                {
                    TokenUrl = new Uri(tokenUrl)
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Keycloak"
                    }
                },
                Array.Empty<string>()
            }
        });

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"NetStone API {version.ToString(3)}",
                    Version = description.ApiVersion.ToString()
                });
        }
    }

    /// <summary>
    ///     Configure Swagger Options. Inherited from the Interface
    /// </summary>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}