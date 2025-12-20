namespace NetStone.Api;

/// <summary>
///     Environment variables, to avoid typos.
/// </summary>
internal static class EnvironmentVariables
{
    /// <summary>
    ///     Used for JWT Token validation.
    /// </summary>
    public const string AuthAudience = "AUTH_AUDIENCE";

    /// <summary>
    ///     Used for JWT Token validation.
    /// </summary>
    public const string AuthAuthority = "AUTH_AUTHORITY";

    /// <summary>
    ///     Scopes to be sent during client credentials authentication through Swagger UI.
    /// </summary>
    public const string SwaggerScopes = "SWAGGER_SCOPES";

    /// <summary>
    ///     If true, Prometheus metrics data will be available through the /metrics endpoint.
    /// </summary>
    public const string MetricsEnabled = "METRICS_ENABLED";

    /// <summary>
    ///     Optional OpenTelemetry tracing endpoint. If set, tracing data will be send to this endpoint.
    /// </summary>
    public const string OtelEndpointUri = "OTEL_ENDPOINT_URI";

    /// <summary>
    ///     PEM formatted X.509 Certificate to encrypt and decrypt data encryption keys with.
    /// </summary>
    /// <remarks>
    ///     Use path + file name WITHOUT extension. .pem and .key extensions for both files will be added automatically.
    ///     See compose.yml for reference.
    /// </remarks>
    public const string DataProtectionCertificate = "DATA_PROTECTION_CERTIFICATE";

    /// <summary>
    ///     Optional secondary PEM formatted X.509 certificate to decrypt data encryption keys with.
    /// </summary>
    /// <remarks>
    ///     When generating a new certificate and replacing <see cref="DataProtectionCertificate" />,
    ///     set this to be the previous certificate.
    ///     New keys will be generated using the new certificate while
    ///     this older certificate will still be available for keys that are still in use by older cookies etc.
    ///     Use path + file name WITHOUT extension. .pem and .key extensions for both files will be added automatically.
    /// </remarks>
    public const string DataProtectionCertificateAlt = "DATA_PROTECTION_CERTIFICATE_ALT";
}