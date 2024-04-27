using Microsoft.Extensions.Configuration;
using NetStone.Common.Exceptions;

namespace NetStone.Common.Extensions;

public static class ConfigurationExtension
{
    public static string GetGuardedConfiguration(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EnvironmentVariableNotSetException(key);
        }

        return value;
    }
}