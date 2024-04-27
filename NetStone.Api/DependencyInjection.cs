using NetStone.Api.Interfaces;
using NetStone.Api.Services;

namespace NetStone.Api;

internal static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddTransient<IFreeCompanyService, FreeCompanyService>();
    }
}