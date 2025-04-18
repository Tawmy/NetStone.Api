using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetStone.Common.DTOs.Character;
using NetStone.Data.Interfaces;
using NetStone.Queue.Interfaces;

namespace NetStone.Queue.Services;

internal class CharacterEventSubscriber(IServiceProvider serviceProvider, ICharacterEventService characterEventService)
    : ICharacterEventSubscriber, IHostedService
{
    public Task CharacterRefreshedAsync(CharacterDtoV2 characterDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterRefreshedAsync(characterDto);
    }

    public Task CharacterRefreshedAsync(CharacterDtoV3 characterDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterRefreshedAsync(characterDto);
    }

    public Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV2 characterClassJobsDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterClassJobsRefreshedAsync(characterClassJobsDto);
    }

    public Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 characterClassJobsDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterClassJobsRefreshedAsync(characterClassJobsDto);
    }

    public Task CharacterMountsRefreshedAsync(CollectionDtoV2<CharacterMountDto> characterMountDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMountsRefreshedAsync(characterMountDto);
    }

    public Task CharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> characterMountDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMountsRefreshedAsync(characterMountDto);
    }

    public Task CharacterMinionsRefreshedAsync(CollectionDtoV2<CharacterMinionDto> characterMinionDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMinionsRefreshedAsync(characterMinionDto);
    }

    public Task CharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> characterMinionDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterMinionsRefreshedAsync(characterMinionDto);
    }

    public Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV2 characterAchievementDto)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IRabbitMqSenderService>();
        return service.SendGetCharacterAchievementsRefreshedAsync(characterAchievementDto);
    }

    public Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 characterAchievementDto)
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