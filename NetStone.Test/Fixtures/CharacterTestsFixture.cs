using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Api.Controllers.V2;
using NetStone.Cache.Db;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;
using NetStone.Data;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Fixtures;

public class CharacterTestsFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddAutoMapper(typeof(CharacterController).Assembly, typeof(DatabaseContext).Assembly,
            typeof(DependencyInjection).Assembly);
        services.AddSingleton<LodestoneClient>(_ =>
        {
            var clientTask = LodestoneClient.GetClientAsync();
            clientTask.Wait();
            return clientTask.Result;
        });
        services.AddScoped<INetStoneService, NetStoneService>();
        services.AddScoped<CharacterGearService>();
        services.AddScoped<CharacterClassJobsService>();
        services.AddScoped<IAutoMapperService, AutoMapperService>();
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