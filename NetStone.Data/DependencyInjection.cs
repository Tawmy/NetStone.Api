using Microsoft.Extensions.DependencyInjection;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;

namespace NetStone.Data;

public static class DependencyInjection
{
    public static async Task AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton(await LodestoneClient.GetClientAsync());
        services.AddSingleton<CollectionDataService>();

        services.AddScoped<ICharacterServiceV4, CharacterServiceV4>();
        services.AddScoped<IFreeCompanyServiceV4, FreeCompanyServiceV4>();
        services.AddScoped<ICharacterServiceV3, CharacterServiceV3>();
        services.AddScoped<IFreeCompanyServiceV3, FreeCompanyServiceV3>();

        services.AddSingleton<ICharacterEventService, CharacterEventService>();
        services.AddSingleton<IFreeCompanyEventService, FreeCompanyEventService>();
    }
}