using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;

namespace NetStone.Cache;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped<INetStoneService, NetStoneService>();
        services.AddScoped<ICharacterCachingServiceV3, CharacterCachingServiceV3>();
        services.AddScoped<IFreeCompanyCachingServiceV3, FreeCompanyCachingServiceV3>();
    }
}