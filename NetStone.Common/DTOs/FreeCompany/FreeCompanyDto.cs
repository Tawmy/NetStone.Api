using NetStone.Common.Enums;

namespace NetStone.Common.DTOs.FreeCompany;

public record FreeCompanyDto
{
    public required string Name { get; init; }
    public required string Id { get; init; }
    public required string World { get; init; }
    public required string Tag { get; init; }
    public required string Slogan { get; init; }

    public required FreeCompanyCrestDto CrestLayers { get; init; }

    public required DateTime Formed { get; init; }
    public required GrandCompany GrandCompany { get; init; }
    public required short Rank { get; init; }
    public short? RankingMonthly { get; init; }
    public short? RankingWeekly { get; init; }
    public required string Recruitment { get; init; }
    public required short ActiveMemberCount { get; init; }
    public required string ActiveState { get; init; }

    public FreeCompanyEstateDto? Estate { get; init; }

    public required bool Cached { get; init; }
    public required DateTime LastUpdated { get; init; }

    public required IEnumerable<FreeCompanyFocusDto> Focus { get; init; }

    public required IEnumerable<FreeCompanyReputationDto> Reputation { get; init; }
}