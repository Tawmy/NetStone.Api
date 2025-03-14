using System.Diagnostics;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;

namespace NetStone.Data.Services;

internal class FreeCompanyService(
    INetStoneService netStoneService,
    IFreeCompanyCachingService cachingService,
    IFreeCompanyEventService eventService,
    IAutoMapperService mapper)
    : IFreeCompanyService
{
    private static readonly ActivitySource ActivitySource = new(nameof(IFreeCompanyService));

    public async Task<FreeCompanySearchPageDto> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();

        var lodestoneQuery = mapper.Map<Search.FreeCompany.FreeCompanySearchQuery>(query);
        var result = await netStoneService.SearchFreeCompany(lodestoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return mapper.Map<FreeCompanySearchPageDto>(result);
    }

    public async Task<FreeCompanyDto> GetFreeCompanyAsync(string lodestoneId, int? maxAge)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedFcDto = await cachingService.GetFreeCompanyAsync(lodestoneId);

        if (cachedFcDto is not null &&
            (DateTime.UtcNow - cachedFcDto.LastUpdated).TotalMinutes <= (maxAge ?? int.MaxValue))
        {
            // return cached fc if possible
            return cachedFcDto with { Cached = true };
        }

        var lodestoneFc = await netStoneService.GetFreeCompany(lodestoneId);
        if (lodestoneFc is null) throw new NotFoundException();

        // cache fc and return
        cachedFcDto = await cachingService.CacheFreeCompanyAsync(lodestoneFc);
        _ = eventService.FreeCompanyRefreshedAsync(cachedFcDto);
        return cachedFcDto;
    }

    public async Task<FreeCompanyMembersOuterDto> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge)
    {
        using var activity = ActivitySource.StartActivity();

        var (cachedMembers, lastUpdated) = await cachingService.GetFreeCompanyMembersAsync(lodestoneId);

        if (cachedMembers.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if free company cached before, last time members were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new FreeCompanyMembersOuterDto(cachedMembers, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Free company was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new FreeCompanyMembersOuterDto(cachedMembers, true, null);
            }
        }

        var lodestoneMembersOuter = await netStoneService.GetFreeCompanyMembers(lodestoneId);

        if (lodestoneMembersOuter is null || !lodestoneMembersOuter.HasResults || !lodestoneMembersOuter.Members.Any())
        {
            throw new NotFoundException();
        }

        var lodestoneMembers = lodestoneMembersOuter.Members.ToList();

        if (lodestoneMembersOuter is { HasResults: true, NumPages: > 1 })
        {
            for (var i = 2; i <= lodestoneMembersOuter.NumPages; i++)
            {
                var lodestoneMembersOuter2 = await netStoneService.GetFreeCompanyMembers(lodestoneId, i);
                if (lodestoneMembersOuter2?.HasResults == true)
                {
                    lodestoneMembers.AddRange(lodestoneMembersOuter2.Members);
                }
            }
        }

        cachedMembers = await cachingService.CacheFreeCompanyMembersAsync(lodestoneId, lodestoneMembers);
        var outerDto = new FreeCompanyMembersOuterDto(cachedMembers, false, DateTime.UtcNow);
        _ = eventService.FreeCompanyMembersRefreshedAsync(outerDto);
        return outerDto;
    }
}