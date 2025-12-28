using Amazon.Runtime;
using Amazon.S3;
using AspNetCoreExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;

namespace NetStone.Cache;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();

        var s3Config = new AmazonS3Config
        {
            ServiceURL = $"https://{config.GetGuardedConfiguration(EnvironmentVariables.S3ServiceUrl)}",
            AuthenticationRegion = config.GetGuardedConfiguration(EnvironmentVariables.S3Region),
            ForcePathStyle = true
        };
        services.AddSingleton<IAmazonS3>(new AmazonS3Client(new EnvironmentVariablesAWSCredentials(), s3Config));
        services.AddSingleton<IS3Service, S3Service>();

        services.AddScoped<INetStoneService, NetStoneService>();
        services.AddScoped<ICharacterCachingService, CharacterCachingService>();
        services.AddScoped<IFreeCompanyCachingService, FreeCompanyCachingService>();
    }
}