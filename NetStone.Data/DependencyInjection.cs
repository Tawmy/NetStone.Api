using Microsoft.Extensions.DependencyInjection;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;

namespace NetStone.Data;

public static class DependencyInjection
{
    public static void AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<LodestoneClient>(_ =>
        {
            var clientTask = LodestoneClient.GetClientAsync();
            clientTask.Wait();
            return clientTask.Result;
        });

        services.AddTransient<ICharacterService, CharacterService>();
        services.AddTransient<IFreeCompanyService, FreeCompanyService>();
    }
}