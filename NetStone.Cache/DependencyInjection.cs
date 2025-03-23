using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;

namespace NetStone.Cache;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped<IAutoMapperService, AutoMapperService>();
        services.AddScoped<INetStoneService, NetStoneService>();
        services.AddScoped<ICharacterCachingServiceV3, CharacterCachingServiceV3>();
        services.AddScoped<ICharacterCachingServiceV2, CharacterCachingServiceV2>();
        services.AddScoped<IFreeCompanyCachingServiceV3, FreeCompanyCachingServiceV3>();
        services.AddScoped<IFreeCompanyCachingServiceV2, FreeCompanyCachingServiceV2>();
        services.AddScoped<CharacterGearServiceV2>();
        services.AddScoped<CharacterClassJobsServiceV2>();
    }
}