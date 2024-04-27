using NetStone.Common.DTOs.Character;

namespace NetStone.Queue.Interfaces;

public interface IRabbitMqSenderService
{
    Task SendGetCharacterSuccessfulAsync(CharacterDto dto);
    Task SendGetCharacterFailedAsync(string lodestoneId, string error);
}