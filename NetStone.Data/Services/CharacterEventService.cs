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

    public Task CharacterRefreshedAsync(CharacterDtoV3 characterDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterRefreshedAsync(characterDto)));
    }

    public Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 characterClassJobsDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterClassJobsRefreshedAsync(characterClassJobsDto)));
    }

    public Task CharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> characterMountDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterMountsRefreshedAsync(characterMountDto)));
    }

    public Task CharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> characterMinionDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterMinionsRefreshedAsync(characterMinionDto)));
    }

    public Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 characterAchievementDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.CharacterAchievementsRefreshedAsync(characterAchievementDto)));
    }
}