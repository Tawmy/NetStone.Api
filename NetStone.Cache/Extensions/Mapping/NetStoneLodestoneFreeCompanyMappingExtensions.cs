using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.FreeCompany;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneLodestoneFreeCompanyMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneLodestoneFreeCompanyMappingExtensions));

    public static FreeCompany ToDb(this LodestoneFreeCompany source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompany
        {
            LodestoneId = source.Id,
            Name = source.Name,
            Slogan = source.Slogan,
            Tag = source.Tag,

            World = source.World,

            CrestTop = source.CrestLayers.TopLayer?.ToString(),
            CrestMiddle = source.CrestLayers.MiddleLayer?.ToString(),
            CrestBottom = source.CrestLayers.BottomLayer?.ToString(),

            Formed = source.Formed,
            GrandCompany = source.GetGrandCompany(),
            Rank = (short)source.Rank,
            RankingMonthly = (short?)source.RankingMonthly,
            RankingWeekly = (short?)source.RankingWeekly,
            Recruitment = source.Recruitment,
            ActiveMemberCount = (short)source.ActiveMemberCount,
            ActiveState = source.ActiveState,

            EstateName = source.Estate?.Name,
            EstateGreeting = source.Estate?.Greeting,
            EstatePlot = source.Estate?.Plot,

            Focus = source.Focus.ToEnum(),

            MaelstromProgress = (short)source.Reputation.Maelstrom.Progress,
            MaelstromRank = source.Reputation.Maelstrom.Rank,
            TwinAdderProgress = (short)source.Reputation.Adders.Progress,
            TwinAdderRank = source.Reputation.Adders.Rank,
            ImmortalFlamesProgress = (short)source.Reputation.Flames.Progress,
            ImmortalFlamesRank = source.Reputation.Flames.Rank
        };
    }

    public static void ToDb(this LodestoneFreeCompany source, FreeCompany target)
    {
        target.Name = source.Name;
        target.Slogan = source.Slogan;
        target.Tag = source.Tag;

        target.CrestTop = source.CrestLayers.TopLayer?.ToString();
        target.CrestMiddle = source.CrestLayers.MiddleLayer?.ToString();
        target.CrestBottom = source.CrestLayers.BottomLayer?.ToString();

        target.GrandCompany = source.GetGrandCompany();
        target.Rank = (short)source.Rank;
        target.RankingMonthly = (short?)source.RankingMonthly;
        target.RankingWeekly = (short?)source.RankingWeekly;
        target.Recruitment = source.Recruitment;
        target.ActiveMemberCount = (short)source.ActiveMemberCount;
        target.ActiveState = source.ActiveState;

        target.EstateName = source.Estate?.Name;
        target.EstateGreeting = source.Estate?.Greeting;
        target.EstatePlot = source.Estate?.Plot;

        target.Focus = source.Focus.ToEnum();

        target.MaelstromProgress = (short)source.Reputation.Maelstrom.Progress;
        target.MaelstromRank = source.Reputation.Maelstrom.Rank;
        target.TwinAdderProgress = (short)source.Reputation.Adders.Progress;
        target.TwinAdderRank = source.Reputation.Adders.Rank;
        target.ImmortalFlamesProgress = (short)source.Reputation.Flames.Progress;
        target.ImmortalFlamesRank = source.Reputation.Flames.Rank;
    }

    private static GrandCompany GetGrandCompany(this LodestoneFreeCompany freeCompany)
    {
        return freeCompany.GrandCompany.ToLowerInvariant() switch
        {
            "maelstrom" => GrandCompany.Maelstrom,
            "order of the twin adder" => GrandCompany.OrderOfTheTwinAdder,
            "immortal flames" => GrandCompany.ImmortalFlames,
            _ => GrandCompany.NoAffiliation
        };
    }
}