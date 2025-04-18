using System.Diagnostics;
using System.Text.Json;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;

namespace NetStone.Data.Services;

public class FreeCompanyServiceV2(
    INetStoneService netStoneService,
    IFreeCompanyCachingServiceV2 cachingService,
    IFreeCompanyEventService eventService,
    IAutoMapperService mapper)
    : IFreeCompanyServiceV2
{
    private static readonly ActivitySource ActivitySource = new(nameof(IFreeCompanyServiceV2));

    public async Task<FreeCompanySearchPageDto> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(FreeCompanySearchQuery), JsonSerializer.Serialize(query));

        var lodestoneQuery = mapper.Map<Search.FreeCompany.FreeCompanySearchQuery>(query);
        var result = await netStoneService.SearchFreeCompany(lodestoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return mapper.Map<FreeCompanySearchPageDto>(result);
    }

    public async Task<FreeCompanyDtoV2> GetFreeCompanyAsync(string lodestoneId, int? maxAge)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedFcDto = await cachingService.GetFreeCompanyAsync(lodestoneId);

        if (cachedFcDto is not null)
        {
            if (cachedFcDto.LastUpdated is null)
            {
                throw new InvalidOperationException($"{nameof(FreeCompanyDtoV2.LastUpdated)} must never be null here.");
            }

            if ((DateTime.UtcNow - cachedFcDto.LastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
            {
                // return cached fc if possible
                cachedFcDto = cachedFcDto with { Cached = true };
                return mapper.Map<FreeCompanyDtoV2>(cachedFcDto);
            }
        }

        var lodestoneFc = await netStoneService.GetFreeCompany(lodestoneId);
        if (lodestoneFc is null) throw new NotFoundException();

        // cache fc and return
        cachedFcDto = await cachingService.CacheFreeCompanyAsync(lodestoneFc);
        _ = eventService.FreeCompanyRefreshedAsync(cachedFcDto);
        return mapper.Map<FreeCompanyDtoV2>(cachedFcDto);
    }

    public async Task<FreeCompanyMembersOuterDtoV2> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge)
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
                    return new FreeCompanyMembersOuterDtoV2(cachedMembers, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Free company was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new FreeCompanyMembersOuterDtoV2(cachedMembers, true, null);
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
                if (lodestoneMembersOuter2?.HasResults is true)
                {
                    lodestoneMembers.AddRange(lodestoneMembersOuter2.Members);
                }
            }
        }

        cachedMembers = await cachingService.CacheFreeCompanyMembersAsync(lodestoneId, lodestoneMembers);
        var outerDto = new FreeCompanyMembersOuterDtoV2(cachedMembers, false, DateTime.UtcNow);
        _ = eventService.FreeCompanyMembersRefreshedAsync(outerDto);
        return outerDto;
    }
}