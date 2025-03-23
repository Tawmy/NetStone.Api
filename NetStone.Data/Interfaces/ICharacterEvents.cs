using NetStone.Common.DTOs.Character;

namespace NetStone.Data.Interfaces;

public interface ICharacterEvents
{
    Task CharacterRefreshedAsync(CharacterDtoV2 characterDto);
    Task CharacterRefreshedAsync(CharacterDtoV3 characterDto);

    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV2 characterClassJobsDto);
    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 characterClassJobsDto);

    Task CharacterMountsRefreshedAsync(CollectionDtoV2<CharacterMountDto> characterMountDto);
    Task CharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> characterMountDto);

    Task CharacterMinionsRefreshedAsync(CollectionDtoV2<CharacterMinionDto> characterMinionDto);
    Task CharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> characterMinionDto);

    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV2 characterAchievementDto);
    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 characterAchievementDto);
}