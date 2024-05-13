using NetStone.Api.Interfaces;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Search.FreeCompany;
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

    public async Task<FreeCompanyMembers> GetFreeCompanyMembersAsync(string lodestoneId, int page)
    {
        var result = await _client.GetFreeCompanyMembers(lodestoneId, page);
        if (result == null) throw new NotFoundException();

        return result;
    }
}