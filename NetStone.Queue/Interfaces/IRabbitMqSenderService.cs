using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Queue.Interfaces;

public interface IRabbitMqSenderService
{
    #region Character

    Task SendGetCharacterSuccessfulAsync(CharacterDto dto);
    Task SendGetCharacterFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterClassJobsSuccessfulAsync(CharacterClassJobOuterDto dto);
    Task SendGetCharacterClassJobsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMinionsSuccessfulAsync(CollectionDto<CharacterMinionDto> dto);
    Task SendGetCharacterMinionsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMountsSuccessfulAsync(CollectionDto<CharacterMountDto> dto);
    Task SendGetCharacterMountsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterAchievementsSuccessfulAsync(CharacterAchievementOuterDto dto);
    Task SendGetCharacterAchievementsFailedAsync(string lodestoneId, string error);

    #endregion

    #region Free Company

    Task SendGetFreeCompanySuccessfulAsync(FreeCompanyDto dto);
    Task SendGetFreeCompanyFailedAsync(string lodestoneId, string error);

    Task SendGetFreeCompanyMembersSuccessfulAsync(FreeCompanyMembersOuterDto dto);
    Task SendGetFreeCompanyMembersFailedAsync(string lodestoneId, string error);

    #endregion
}