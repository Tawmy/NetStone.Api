using Microsoft.Extensions.DependencyInjection;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;

namespace NetStone.Data;

public static class DependencyInjection
{
    public static async Task AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton(await LodestoneClient.GetClientAsync());

        services.AddTransient<ICharacterService, CharacterService>();
        services.AddTransient<IFreeCompanyService, FreeCompanyService>();

        services.AddTransient<ILegacyCharacterService, LegacyCharacterService>();

        services.AddSingleton<ICharacterEventService, CharacterEventService>();
    }
}