using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Api.Controllers.V2;
using NetStone.Cache.Db;
using NetStone.Data.Interfaces;
using NSubstitute;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Fixtures;

public class FreeCompanyComparisonsFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddAutoMapper(typeof(FreeCompanyController).Assembly, typeof(DatabaseContext).Assembly);
        services.AddSingleton<LodestoneClient>(_ =>
        {
            var clientTask = LodestoneClient.GetClientAsync();
            clientTask.Wait();
            return clientTask.Result;
        });

        services.AddScoped<IFreeCompanyEventService>(_ => Substitute.For<IFreeCompanyEventService>());
    }

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new TestAppSettings { Filename = "appsettings.json", IsOptional = false };
    }

    protected override ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }
}