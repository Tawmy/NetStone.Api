using NetStone.Common.DTOs.Character;

namespace NetStone.Queue.Interfaces;

public interface IRabbitMqSenderService
{
    Task SendGetCharacterSuccessfulAsync(CharacterDto dto);
    Task SendGetCharacterFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterClassJobsSuccessfulAsync(CharacterClassJobOuterDto dto);
    Task SendGetCharacterClassJobsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMinionsSuccessfulAsync(CharacterMinionOuterDto dto);
    Task SendGetCharacterMinionsFailedAsync(string lodestoneId, string error);

    Task SendGetCharacterMountsSuccessfulAsync(CharacterMountOuterDto dto);
    Task SendGetCharacterMountsFailedAsync(string lodestoneId, string error);
}