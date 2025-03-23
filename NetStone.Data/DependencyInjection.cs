using Microsoft.Extensions.DependencyInjection;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;

namespace NetStone.Data;

public static class DependencyInjection
{
    public static async Task AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton(await LodestoneClient.GetClientAsync());

        services.AddScoped<ICharacterServiceV3, CharacterServiceV3>();
        services.AddScoped<IFreeCompanyServiceV3, FreeCompanyServiceV3>();

        services.AddScoped<ICharacterServiceV2, CharacterServiceV2>();
        services.AddScoped<IFreeCompanyServiceV2, FreeCompanyServiceV2>();

        services.AddSingleton<ICharacterEventService, CharacterEventService>();
        services.AddSingleton<IFreeCompanyEventService, FreeCompanyEventService>();
    }
}