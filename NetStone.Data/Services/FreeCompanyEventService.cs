using NetStone.Common.DTOs.FreeCompany;
using NetStone.Data.Interfaces;

namespace NetStone.Data.Services;

public class FreeCompanyEventService : IFreeCompanyEventService
{
    private readonly HashSet<IFreeCompanyEventSubscriber> _subscribers = [];

    public void Subscribe(IFreeCompanyEventSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unsubscribe(IFreeCompanyEventSubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public Task FreeCompanyRefreshedAsync(FreeCompanyDtoV2 freeCompanyDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.FreeCompanyRefreshedAsync(freeCompanyDto)));
    }

    public Task FreeCompanyRefreshedAsync(FreeCompanyDtoV3 freeCompanyDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.FreeCompanyRefreshedAsync(freeCompanyDto)));
    }

    public Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV2 freeCompanyMemberDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.FreeCompanyMembersRefreshedAsync(freeCompanyMemberDto)));
    }

    public Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 freeCompanyMemberDto)
    {
        return Task.WhenAll(_subscribers.Select(x => x.FreeCompanyMembersRefreshedAsync(freeCompanyMemberDto)));
    }
}