using MassTransit;
using NetStone.Common.DTOs.Character;
using NetStone.Queue.Interfaces;

namespace NetStone.Queue.Services;

public class RabbitMqSenderService(ISendEndpointProvider provider) : IRabbitMqSenderService
{
    public async Task SendGetCharacterSuccessfulAsync(CharacterDto dto)
    {
        var uri = new Uri("queue:netstone-get-character-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterClassJobsSuccessfulAsync(CharacterClassJobOuterDto dto)
    {
        var uri = new Uri("queue:netstone-get-character-class-jobs-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterClassJobsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-class-jobs-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }
}