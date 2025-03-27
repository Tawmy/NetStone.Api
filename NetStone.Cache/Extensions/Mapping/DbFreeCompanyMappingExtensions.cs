using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbFreeCompanyMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbFreeCompanyMappingExtensions));

    public static FreeCompanyDtoV3 ToDto(this FreeCompany source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompanyDtoV3
        {
            Name = source.Name,
            Id = source.LodestoneId,
            World = source.World,
            Tag = source.Tag,
            Slogan = source.Slogan,

            CrestLayers = new FreeCompanyCrestDto
            {
                TopLayer = source.CrestTop,
                MiddleLayer = source.CrestMiddle,
                BottomLayer = source.CrestBottom
            },

            Formed = source.Formed,
            GrandCompany = source.GrandCompany,
            Rank = source.Rank,
            RankingMonthly = source.RankingMonthly,
            RankingWeekly = source.RankingWeekly,
            Recruitment = source.Recruitment,
            ActiveMemberCount = source.ActiveMemberCount,
            ActiveState = source.ActiveState,

            Estate = source is { EstateName: not null, EstateGreeting: not null, EstatePlot: not null }
                ? new FreeCompanyEstateDto(source.EstateName, source.EstateGreeting, source.EstatePlot)
                : null,

            Focus = source.GetFocus(),
            Reputation = source.GetReputation(),

            LastUpdated = source.FreeCompanyUpdatedAt
        };
    }

    private static IEnumerable<FreeCompanyFocusDto> GetFocus(this FreeCompany source)
    {
        return Enum.GetValues<FreeCompanyFocus>().Cast<Enum>().Where(x =>
                !Equals((int)(object)x, 0) && // filter out empty value
                source.Focus.HasFlag(x))
            .Select(x =>
                new FreeCompanyFocusDto((FreeCompanyFocus)x, x.TryGetDisplayName(out var value) ? value! : x.ToString(),
                    ((FreeCompanyFocus)x).GetFocusIcon()));
    }

    private static IEnumerable<FreeCompanyReputationDto> GetReputation(this FreeCompany source)
    {
        return new List<FreeCompanyReputationDto>
        {
            new(GrandCompany.Maelstrom, source.MaelstromRank, source.MaelstromProgress),
            new(GrandCompany.OrderOfTheTwinAdder, source.TwinAdderRank,
                source.TwinAdderProgress),
            new(GrandCompany.ImmortalFlames, source.ImmortalFlamesRank, source.ImmortalFlamesProgress)
        };
    }
}