using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;

namespace NetStone.Cache;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddScoped<INetStoneService, NetStoneService>();
        services.AddScoped<ICharacterCachingService, CharacterCachingService>();
        services.AddScoped<IFreeCompanyCachingService, FreeCompanyCachingService>();
    }
}