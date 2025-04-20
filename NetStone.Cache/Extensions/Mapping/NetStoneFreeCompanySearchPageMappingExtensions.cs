using System.Diagnostics;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.Search.FreeCompany;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneFreeCompanySearchPageMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneFreeCompanySearchPageMappingExtensions));

    public static FreeCompanySearchPageDto ToDto(this FreeCompanySearchPage source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompanySearchPageDto(source.HasResults, source.Results.Select(x => x.ToDto()),
            source.CurrentPage, source.NumPages);
    }
}