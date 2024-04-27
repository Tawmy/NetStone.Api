using Microsoft.Extensions.DependencyInjection;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;

namespace NetStone.Data;

public static class DependencyInjection
{
    public static void AddDataServices(this IServiceCollection services)
    {
        services.AddTransient<ICharacterService, CharacterService>();
    }
}