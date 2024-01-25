using NetStone.Api.Exceptions;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Search.FreeCompany;

namespace NetStone.Api.Services;

internal class FreeCompanyService : IFreeCompanyService
{
    private readonly LodestoneClient _client;

    public FreeCompanyService(LodestoneClient client)
    {
        _client = client;
    }

    public async Task<FreeCompanySearchPage> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page)
    {
        var result = await _client.SearchFreeCompany(query, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result;
    }

    public async Task<LodestoneFreeCompany> GetFreeCompanyAsync(string lodestoneId)
    {
        var result = await _client.GetFreeCompany(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<FreeCompanyMembers> GetFreeCompanyMembersAsync(string lodestoneId, int page)
    {
        var result = await _client.GetFreeCompanyMembers(lodestoneId, page);
        if (result == null) throw new NotFoundException();

        return result;
    }
}