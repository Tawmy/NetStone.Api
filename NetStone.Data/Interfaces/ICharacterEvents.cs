using NetStone.Common.DTOs.Character;

namespace NetStone.Data.Interfaces;

public interface ICharacterEvents
{
    Task CharacterRefreshedAsync(CharacterDtoV3 characterDto);
    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 characterClassJobsDto);
    Task CharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> characterMountDto);
    Task CharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> characterMinionDto);
    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 characterAchievementDto);
}