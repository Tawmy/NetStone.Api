using NetStone.Common.Enums;

namespace NetStone.Common.DTOs.Character;

public record CharacterDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Server { get; init; }
    public string? Title { get; init; }
    public required string Avatar { get; init; }
    public required string Portrait { get; init; }
    public required string Bio { get; init; }
    public required string Nameday { get; init; }

    public required ClassJob ActiveClassJob { get; init; }
    public required short ActiveClassJobLevel { get; init; }
    public required string ActiveClassJobIcon { get; init; }

    public GrandCompany GrandCompany { get; init; }
    public string? GrandCompanyRank { get; init; }

    public CharacterFreeCompanyDto? FreeCompany { get; init; }

    public required string GuardianDeityName { get; init; }
    public required string GuardianDeityIcon { get; init; }

    // TODO expand PVP Team to be full entity?
    public string? PvpTeam { get; init; }
    public required string RaceClanGender { get; init; }

    public string? TownName { get; init; }
    public string? TownIcon { get; init; }

    public required IEnumerable<CharacterGearDto> Gear { get; init; }

    public required CharacterAttributesDto Attributes { get; init; }

    public bool Cached { get; init; }
    public DateTime LastUpdated { get; init; }
}