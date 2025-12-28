using Asp.Versioning.ApiExplorer;
using AspNetCoreExtensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetStone.Api;

internal class ConfigureSwaggerOptions(
    IConfiguration configuration,
    IApiVersionDescriptionProvider provider,
    Version version)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    private const string SecurityScheme = "Keycloak";

    /// <summary>
    ///     Configure each API discovered for Swagger Documentation
    /// </summary>
    public void Configure(SwaggerGenOptions options)
    {
        // Get environment variables for Swagger auth
        var authority = configuration.GetGuardedConfiguration(EnvironmentVariables.AuthAuthority);
        var scopes = configuration[EnvironmentVariables.SwaggerScopes] ?? string.Empty;

        var scopesDict = scopes.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToDictionary(x => x, _ => "Enable me!");

        var tokenUrl = Path.Combine(authority, "protocol/openid-connect/token");

        // Add Keycloak auth to Swagger UI
        options.AddSecurityDefinition(SecurityScheme, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow
                {
                    TokenUrl = new Uri(tokenUrl),
                    Scopes = scopesDict
                }
            }
        });

        options.AddSecurityRequirement(x => new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference(SecurityScheme, x), [] }
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