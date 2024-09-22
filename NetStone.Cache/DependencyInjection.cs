using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;

namespace NetStone.Cache;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddTransient<ICharacterCachingService, CharacterCachingService>();
        services.AddTransient<IFreeCompanyCachingService, FreeCompanyCachingService>();
        services.AddTransient<CharacterGearService>();
        services.AddTransient<CharacterClassJobsService>();
    }
}