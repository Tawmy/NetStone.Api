using NetStone.Api.Services;
using NetStone.Queue.Interfaces;

namespace NetStone.Api;

internal static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddTransient<IFreeCompanyService, FreeCompanyService>();
    }
}