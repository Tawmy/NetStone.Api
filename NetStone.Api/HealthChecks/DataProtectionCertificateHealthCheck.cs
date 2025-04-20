using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NetStone.Common.Extensions;

namespace NetStone.Api.HealthChecks;

internal class DataProtectionCertificateHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var certificatePath = configuration.GetGuardedConfiguration(EnvironmentVariables.DataProtectionCertificate);
        var certificate = X509Certificate2.CreateFromPemFile($"{certificatePath}.pem", $"{certificatePath}.key");

        if (certificate.NotBefore > DateTime.Now)
        {
            // cert not valid yet
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }

        if (certificate.NotAfter < DateTime.Now.AddDays(30))
        {
            // cert expires in less than 30 days
            return Task.FromResult(HealthCheckResult.Degraded());
        }

        if (certificate.NotAfter < DateTime.Now)
        {
            // cert has expired
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }

        return Task.FromResult(HealthCheckResult.Healthy());
    }
}