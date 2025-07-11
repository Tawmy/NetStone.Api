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

public class FreeCompanyService(
    INetStoneService netStoneService,
    IFreeCompanyCachingService cachingService,
    IFreeCompanyEventService eventService,
    ILogger<FreeCompanyService> logger)
    : IFreeCompanyService
{
    private static readonly ActivitySource ActivitySource = new(nameof(IFreeCompanyService));

    public async Task<FreeCompanySearchPageDto> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(FreeCompanySearchQuery), JsonSerializer.Serialize(query));

        var lodestoneQuery = query.ToNetStone();
        var result = await netStoneService.SearchFreeCompany(lodestoneQuery, page);
        if (result is not { HasResults: true })
        {
            throw new NotFoundException();
        }

        if (!result.Results.Any())
        {
            // parser returns HasResults true, but zero results during maintenance
            throw new ParsingFailedException(JsonSerializer.Serialize(query));
        }

        return result.ToDto();
    }

    public async Task<FreeCompanyDto> GetFreeCompanyAsync(string lodestoneId, int? maxAge, FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedFcDto = await cachingService.GetFreeCompanyAsync(lodestoneId);

        if (cachedFcDto is not null)
        {
            if (cachedFcDto.LastUpdated is null)
            {
                throw new InvalidOperationException($"{nameof(FreeCompanyDto.LastUpdated)} must never be null here.");
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

    public async Task<FreeCompanyDto> GetFreeCompanyByNameAsync(string name, string world)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedFcDto = await cachingService.GetFreeCompanyAsync(name, world);

        if (cachedFcDto is null)
        {
            throw new NotFoundException();
        }

        if (cachedFcDto.LastUpdated is null)
        {
            throw new InvalidOperationException($"{nameof(cachedFcDto.LastUpdated)} must never be null here.");
        }

        return cachedFcDto with { Cached = true };
    }

    public async Task<FreeCompanyMembersOuterDto> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge,
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
                return new FreeCompanyMembersOuterDto(cachedMembers, true, lastUpdated, true, ex.Message);
            }

            if (ex is FormatException)
            {
                throw new ParsingFailedException(lodestoneId);
            }

            throw;
        }

        if (lodestoneMembersOuter is null || !lodestoneMembersOuter.HasResults || !lodestoneMembersOuter.Members.Any())
        {
            if (cachedMembers.Any())
            {
                // members have been cached before, throw appropriate error
                throw new ParsingFailedException(lodestoneId);
            }

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
        var outerDto = new FreeCompanyMembersOuterDto(cachedMembers, false, DateTime.UtcNow);
        _ = eventService.FreeCompanyMembersRefreshedAsync(outerDto);
        return outerDto;
    }
}