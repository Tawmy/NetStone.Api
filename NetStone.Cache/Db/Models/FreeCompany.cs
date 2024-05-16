using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class FreeCompany
{
    public int Id { get; set; }

    public required string LodestoneId { get; set; }

    public required string Name { get; set; }
    public required string Slogan { get; set; }
    public required string Tag { get; set; }

    public required string World { get; set; }

    public required string CrestTop { get; set; }
    public required string CrestMiddle { get; set; }
    public required string CrestBottom { get; set; }

    public required DateTime Formed { get; set; }
    public required GrandCompany GrandCompany { get; set; }
    public required short Rank { get; set; }
    public short? RankingMonthly { get; set; }
    public short? RankingWeekly { get; set; }
    public required string Recruitment { get; set; }
    public required short ActiveMemberCount { get; set; }
    public required string ActiveState { get; set; }

    public string? EstateName { get; set; }
    public string? EstateGreeting { get; set; }
    public string? EstatePlot { get; set; }

    public DateTime FreeCompanyUpdatedAt { get; set; }

    public DateTime? FreeCompanyMembersUpdatedAt { get; set; }

    public required FreeCompanyFocus Focus { get; set; }

    public required short MaelstromProgress { get; set; }
    public required string MaelstromRank { get; set; }
    public required short TwinAdderProgress { get; set; }
    public required string TwinAdderRank { get; set; }
    public required short ImmortalFlamesProgress { get; set; }
    public required string ImmortalFlamesRank { get; set; }

    public ICollection<FreeCompanyMember> Members { get; set; } = new HashSet<FreeCompanyMember>();
}