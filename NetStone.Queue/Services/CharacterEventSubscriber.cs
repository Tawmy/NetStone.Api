using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetStone.Common.DTOs.Character;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;

namespace NetStone.Queue.Services;

internal class CharacterEventSubscriber(IServiceProvider serviceProvider, ICharacterEventService characterEventService)
    : ICharacterEventSubscriber, IHostedService
{
    public Task CharacterRefreshedAsync(CharacterDto characterDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterRefreshedAsync(characterDto);
    }

    public Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDto characterClassJobsDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterClassJobsRefreshedAsync(characterClassJobsDto);
    }

    public Task CharacterMountsRefreshedAsync(CollectionDto<CharacterMountDto> characterMountDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMountsRefreshedAsync(characterMountDto);
    }

    public Task CharacterMinionsRefreshedAsync(CollectionDto<CharacterMinionDto> characterMinionDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMinionsRefreshedAsync(characterMinionDto);
    }

    public Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDto characterAchievementDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterAchievementsRefreshedAsync(characterAchievementDto);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        characterEventService.Subscribe(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        characterEventService.Unsubscribe(this);
        return Task.CompletedTask;
    }
}