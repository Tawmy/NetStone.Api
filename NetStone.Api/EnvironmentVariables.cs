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
    ///     Optional OpenTelemetry tracing endpoint. If set, tracing data will be send to this endpoint. 
    /// </summary>
    public const string OtelEndpointUri = "OTEL_ENDPOINT_URI";
}