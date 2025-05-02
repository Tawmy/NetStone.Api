using NetStone.Common.DTOs.Character;

namespace NetStone.Data.Interfaces;

public interface ICharacterEvents
{
    Task CharacterRefreshedAsync(CharacterDto characterDto);
    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDto characterClassJobsDto);
    Task CharacterMountsRefreshedAsync(CollectionDto<CharacterMountDto> characterMountDto);
    Task CharacterMinionsRefreshedAsync(CollectionDto<CharacterMinionDto> characterMinionDto);
    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDto characterAchievementDto);
}