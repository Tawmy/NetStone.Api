using MassTransit;
using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Queue.Interfaces;

namespace NetStone.Queue.Services;

public class RabbitMqSenderService(ISendEndpointProvider provider) : IRabbitMqSenderService
{
    public async Task SendGetCharacterSuccessfulAsync(CharacterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterRefreshedAsync(CharacterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterClassJobsSuccessfulAsync(CharacterClassJobOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-class-jobs-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterClassJobsRefreshedAsync(CharacterClassJobOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-class-jobs-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterClassJobsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-class-jobs-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterMinionsSuccessfulAsync(CollectionDtoV3<CharacterMinionDto> dto)
    {
        var uri = new Uri("exchange:netstone-get-character-minions-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMinionsRefreshedAsync(CollectionDtoV3<CharacterMinionDto> dto)
    {
        var uri = new Uri("exchange:netstone-get-character-minions-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMinionsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-minions-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterMountsSuccessfulAsync(CollectionDtoV3<CharacterMountDto> dto)
    {
        var uri = new Uri("exchange:netstone-get-character-mounts-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMountsRefreshedAsync(CollectionDtoV3<CharacterMountDto> dto)
    {
        var uri = new Uri("exchange:netstone-get-character-mounts-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterMountsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-mounts-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetCharacterAchievementsSuccessfulAsync(CharacterAchievementOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-achievements-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterAchievementsRefreshedAsync(CharacterAchievementOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-character-achievements-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetCharacterAchievementsFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-character-achievements-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetFreeCompanySuccessfulAsync(FreeCompanyDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-free-company-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetFreeCompanyRefreshedAsync(FreeCompanyDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-free-company-refreshed");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetFreeCompanyFailedAsync(string lodestoneId, string error)
    {
        var uri = new Uri("queue:netstone-get-free-company-error");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(new { lodestoneId, error });
    }

    public async Task SendGetFreeCompanyMembersSuccessfulAsync(FreeCompanyMembersOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-free-company-members-result");
        var endpoint = await provider.GetSendEndpoint(uri);
        await endpoint.Send(dto);
    }

    public async Task SendGetFreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 dto)
    {
        var uri = new Uri("exchange:netstone-get-free-company-members-refreshed");
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