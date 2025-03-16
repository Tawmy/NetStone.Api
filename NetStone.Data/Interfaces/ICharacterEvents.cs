using NetStone.Common.DTOs.Character;

namespace NetStone.Data.Interfaces;

public interface ICharacterEvents
{
    Task CharacterRefreshedAsync(CharacterDto characterDto);
    Task CharacterRefreshedAsync(CharacterDtoV3 characterDto);

    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDto characterClassJobsDto);
    Task CharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 characterClassJobsDto);

    Task CharacterMountsRefreshedAsync(CollectionDto<CharacterMountDto> characterMountDto);
    Task CharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> characterMountDto);

    Task CharacterMinionsRefreshedAsync(CollectionDto<CharacterMinionDto> characterMinionDto);
    Task CharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> characterMinionDto);

    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDto characterAchievementDto);
    Task CharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 characterAchievementDto);
}