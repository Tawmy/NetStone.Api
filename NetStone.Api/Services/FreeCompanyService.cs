using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Queue.Interfaces;
using NetStone.Search.FreeCompany;

namespace NetStone.Api.Services;

internal class FreeCompanyService : IFreeCompanyService
{
    private readonly IFreeCompanyCachingService _cachingService;
    private readonly LodestoneClient _client;

    public FreeCompanyService(LodestoneClient client, IFreeCompanyCachingService cachingService)
    {
        _client = client;
        _cachingService = cachingService;
    }

    public async Task<FreeCompanySearchPage> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        var result = await _client.SearchFreeCompany(query, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result;
    }

    public async Task<FreeCompanyDto> GetFreeCompanyAsync(string lodestoneId, int? maxAge)
    {
        var cachedFcDto = await _cachingService.GetFreeCompanyAsync(lodestoneId);

        if (cachedFcDto is not null &&
            (DateTime.UtcNow - cachedFcDto.LastUpdated).TotalMinutes <= (maxAge ?? int.MaxValue))
        {
            // return cached fc if possible
            return cachedFcDto with { Cached = true };
        }

        var lodestoneFc = await _client.GetFreeCompany(lodestoneId);
        if (lodestoneFc is null) throw new NotFoundException();

        // cache fc and return
        return await _cachingService.CacheFreeCompanyAsync(lodestoneFc);
    }

    public async Task<FreeCompanyMembersOuterDto> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge)
    {
        var (cachedMembers, lastUpdated) = await _cachingService.GetFreeCompanyMembersAsync(lodestoneId);

        if (cachedMembers.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if free company cached before, last time memberes were cached can be saved.
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

        var lodestoneMembersOuter = await _client.GetFreeCompanyMembers(lodestoneId);

        if (lodestoneMembersOuter is null || !lodestoneMembersOuter.HasResults || !lodestoneMembersOuter.Members.Any())
        {
            throw new NotFoundException();
        }

        var lodestoneMembers = lodestoneMembersOuter?.Members.ToList() ?? [];

        if (lodestoneMembersOuter is { HasResults: true, NumPages: > 1 })
        {
            for (var i = 2; i <= lodestoneMembersOuter.NumPages; i++)
            {
                var lodestoneMembersOuter2 = await _client.GetFreeCompanyMembers(lodestoneId, i);
                if (lodestoneMembersOuter2?.HasResults == true)
                {
                    lodestoneMembers.AddRange(lodestoneMembersOuter2.Members);
                }
            }
        }

        cachedMembers = await _cachingService.CacheFreeCompanyMembersAsync(lodestoneId, lodestoneMembers);
        return new FreeCompanyMembersOuterDto(cachedMembers, false, DateTime.UtcNow);
    }
}