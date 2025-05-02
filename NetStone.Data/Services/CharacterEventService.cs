using NetStone.Common.DTOs.Character;
using NetStone.Data.Interfaces;

namespace NetStone.Data.Services;

internal class CharacterEventService : ICharacterEventService
{
    private readonly HashSet<ICharacterEventSubscriber> _subscribers = [];

    public void Subscribe(ICharacterEventSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unsubscribe(ICharacterEventSubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public Task CharacterRefreshedAsync(CharacterDto characterDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterRefreshedAsync(characterDto)));
    }

    public Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDto characterClassJobsDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterClassJobsRefreshedAsync(characterClassJobsDto)));
    }

    public Task CharacterMountsRefreshedAsync(CollectionDto<CharacterMountDto> characterMountDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterMountsRefreshedAsync(characterMountDto)));
    }

    public Task CharacterMinionsRefreshedAsync(CollectionDto<CharacterMinionDto> characterMinionDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterMinionsRefreshedAsync(characterMinionDto)));
    }

    public Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDto characterAchievementDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterAchievementsRefreshedAsync(characterAchievementDto)));
    }
}