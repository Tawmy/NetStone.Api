using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetStone.Api.Controllers.V2;
using NetStone.Cache.Db;
using NetStone.Cache.Services;
using NetStone.Data.Interfaces;
using NSubstitute;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Fixtures;

public class CharacterComparisonsFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddAutoMapper(typeof(CharacterController).Assembly, typeof(DatabaseContext).Assembly);
        services.AddSingleton<LodestoneClient>(_ =>
        {
            var clientTask = LodestoneClient.GetClientAsync();
            clientTask.Wait();
            return clientTask.Result;
        });

        services.AddScoped<ICharacterEventService>(_ => Substitute.For<ICharacterEventService>());
        services.AddScoped<CharacterGearService>();
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