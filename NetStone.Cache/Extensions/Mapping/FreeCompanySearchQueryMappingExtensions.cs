using NetStone.Search.FreeCompany;
using NetStone.StaticData;
using FreeCompanySearchQuery = NetStone.Common.Queries.FreeCompanySearchQuery;

namespace NetStone.Cache.Extensions.Mapping;

public static class FreeCompanySearchQueryMappingExtensions
{
    public static Search.FreeCompany.FreeCompanySearchQuery ToNetStone(this FreeCompanySearchQuery source)
    {
        return new Search.FreeCompany.FreeCompanySearchQuery
        {
            Name = source.Name,
            World = source.World ?? string.Empty,
            DataCenter = source.DataCenter ?? string.Empty,
            IsCommunityFinderRecruiting = source.IsCommunityFinderRecruiting,
            ActiveTimes = Enum.Parse<ActiveTimes>(source.ActiveTimes.ToString()),
            ActiveMembers = Enum.Parse<ActiveMembers>(source.ActiveMembers.ToString()),
            Recruitment = Enum.Parse<Recruitment>(source.Recruitment.ToString()),
            Housing = Enum.Parse<Housing>(source.Housing.ToString()),
            Focus = Enum.Parse<Focus>(source.Focus.ToString()),
            Seeking = Enum.Parse<Seeking>(source.Seeking.ToString()),
            GrandCompany = Enum.Parse<GrandCompany>(source.GrandCompany.ToString()),
            SortKind = Enum.Parse<SortKind>(source.SortKind.ToString())
        };
    }
}