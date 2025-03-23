using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Fixtures;

public class FreeCompanyTestsFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddSingleton<LodestoneClient>(_ =>
        {
            var clientTask = LodestoneClient.GetClientAsync();
            clientTask.Wait();
            return clientTask.Result;
        });
        services.AddScoped<INetStoneService, NetStoneService>();
    }

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new TestAppSettings { Filename = "appsettings.json", IsOptional = true };
    }

    protected override ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }
}