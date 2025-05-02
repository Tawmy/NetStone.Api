using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Queue.Interfaces;

public interface IRabbitMqSenderService
{
    #region Character

    Task SendGetCharacterSuccessfulAsync(CharacterDtoV3 dto);
    Task SendGetCharacterRefreshedAsync(CharacterDtoV3 dto);
    Task SendGetCharacterFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterClassJobsSuccessfulAsync(CharacterClassJobOuterDtoV3 dto);
    Task SendGetCharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 dto);
    Task SendGetCharacterClassJobsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMinionsSuccessfulAsync(CollectionDtoV3<CharacterMinionDto> dto);
    Task SendGetCharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> dto);
    Task SendGetCharacterMinionsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMountsSuccessfulAsync(CollectionDtoV3<CharacterMountDto> dto);
    Task SendGetCharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> dto);
    Task SendGetCharacterMountsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterAchievementsSuccessfulAsync(CharacterAchievementOuterDtoV3 dto);
    Task SendGetCharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 dto);
    Task SendGetCharacterAchievementsFailedAsync(string lodestoneId, string error);

    #endregion

    #region Free Company

    Task SendGetFreeCompanySuccessfulAsync(FreeCompanyDtoV3 dto);
    Task SendGetFreeCompanyRefreshedAsync(FreeCompanyDtoV3 dto);
    Task SendGetFreeCompanyFailedAsync(string lodestoneId, string error);

    Task SendGetFreeCompanyMembersSuccessfulAsync(FreeCompanyMembersOuterDtoV3 dto);
    Task SendGetFreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 dto);
    Task SendGetFreeCompanyMembersFailedAsync(string lodestoneId, string error);

    #endregion
}