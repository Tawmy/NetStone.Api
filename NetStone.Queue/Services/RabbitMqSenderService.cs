using MassTransit;
using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;
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

    public async Task SendGetCharacterMinionsSuccessfulAsync(CharacterMinionOuterDto dto)
    {
        var uri = new Uri("queue:netstone-get-character-minions-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMinionsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-minions-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterMountsSuccessfulAsync(CharacterMountOuterDto dto)
    {
        var uri = new Uri("queue:netstone-get-character-mounts-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMountsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-mounts-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetFreeCompanySuccessfulAsync(FreeCompanyDto dto)
    {
        var uri = new Uri("queue:netstone-get-free-company-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetFreeCompanyFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-free-company-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetFreeCompanyMembersSuccessfulAsync(FreeCompanyMembersOuterDto dto)
    {
        var uri = new Uri("queue:netstone-get-free-company-members-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetFreeCompanyMembersFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-free-company-members-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }
}