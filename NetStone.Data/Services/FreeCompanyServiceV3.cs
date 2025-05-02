using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Data.Services;

public class FreeCompanyServiceV3(
    INetStoneService netStoneService,
    IFreeCompanyCachingServiceV3 cachingService,
    IFreeCompanyEventService eventService,
    ILogger<FreeCompanyServiceV3> logger)
    : IFreeCompanyServiceV3
{
    private static readonly ActivitySource ActivitySource = new(nameof(IFreeCompanyServiceV3));

    public async Task<FreeCompanySearchPageDto> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(FreeCompanySearchQuery), JsonSerializer.Serialize(query));

        var lodestoneQuery = query.ToNetStone();
        var result = await netStoneService.SearchFreeCompany(lodestoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result.ToDto();
    }

    public async Task<FreeCompanyDtoV3> GetFreeCompanyAsync(string lodestoneId, int? maxAge, FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedFcDto = await cachingService.GetFreeCompanyAsync(lodestoneId);

        if (cachedFcDto is not null)
        {
            if (cachedFcDto.LastUpdated is null)
            {
                throw new InvalidOperationException($"{nameof(FreeCompanyDtoV3.LastUpdated)} must never be null here.");
            }

            if ((DateTime.UtcNow - cachedFcDto.LastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
            {
                // return cached fc if possible
                return cachedFcDto with { Cached = true };
            }
        }

        LodestoneFreeCompany? lodestoneFc;
        try
        {
            lodestoneFc = await netStoneService.GetFreeCompany(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedFcDto is not null && (useFallback is FallbackType.Any ||
                                            (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetFreeCompanyAsync), ex.Message);
                return cachedFcDto with { Cached = true, FallbackUsed = true, FallbackReason = ex.Message };
            }

            throw;
        }

        if (lodestoneFc is null)
        {
            throw new NotFoundException();
        }

        if (string.IsNullOrWhiteSpace(lodestoneFc.Name))
        {
            if (cachedFcDto is not null && useFallback is FallbackType.Any)
            {
                return cachedFcDto with
                {
                    Cached = true, FallbackUsed = true, FallbackReason = nameof(ParsingFailedException)
                };
            }

            throw new ParsingFailedException(lodestoneId);
        }

        // cache fc and return
        cachedFcDto = await cachingService.CacheFreeCompanyAsync(lodestoneFc);
        _ = eventService.FreeCompanyRefreshedAsync(cachedFcDto);
        return cachedFcDto;
    }

    public async Task<FreeCompanyMembersOuterDtoV3> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge,
        FallbackType useFallback)
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
                    return new FreeCompanyMembersOuterDtoV3(cachedMembers, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Free company was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new FreeCompanyMembersOuterDtoV3(cachedMembers, true, null);
            }
        }

        FreeCompanyMembers? lodestoneMembersOuter;
        try
        {
            lodestoneMembersOuter = await netStoneService.GetFreeCompanyMembers(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedMembers.Any() && (useFallback is FallbackType.Any ||
                                        (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetFreeCompanyMembersAsync), ex.Message);
                return new FreeCompanyMembersOuterDtoV3(cachedMembers, true, lastUpdated, true, ex.Message);
            }

            if (ex is FormatException)
            {
                throw new ParsingFailedException(lodestoneId);
            }

            throw;
        }

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
        var outerDto = new FreeCompanyMembersOuterDtoV3(cachedMembers, false, DateTime.UtcNow);
        _ = eventService.FreeCompanyMembersRefreshedAsync(outerDto);
        return outerDto;
    }
}