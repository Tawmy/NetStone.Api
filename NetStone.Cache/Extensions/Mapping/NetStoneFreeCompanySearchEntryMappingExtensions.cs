using System.Diagnostics;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.Search.FreeCompany;
using ActiveTimes = NetStone.Common.Enums.ActiveTimes;
using Housing = NetStone.Common.Enums.Housing;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneFreeCompanySearchEntryMappingExtensions
{
    private static readonly ActivitySource
        ActivitySource = new(nameof(NetStoneFreeCompanySearchEntryMappingExtensions));

    public static FreeCompanySearchPageResultDto ToDto(this FreeCompanySearchEntry source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompanySearchPageResultDto
        {
            Name = source.Name,
            Id = source.Id ?? throw new InvalidOperationException($"{nameof(source.Id)} must not be null."),
            Server = source.Server,
            Datacenter = source.Datacenter,
            CrestLayers = source.CrestLayers.ToDto(),
            Formed = source.Formed,
            Active = Enum.Parse<ActiveTimes>(source.Active.ToString()),
            ActiveMembers = source.ActiveMembers,
            RecruitmentOpen = source.RecruitmentOpen,
            GrandCompany = source.GrandCompany,
            EstateBuild = Enum.Parse<Housing>(source.EstateBuild.ToString())
        };
    }
}