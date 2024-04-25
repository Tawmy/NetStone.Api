using NetStone.StaticData;

namespace NetStone.Common.DTOs;

public record CharacterDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Server { get; set; }
    public string? Title { get; set; }
    public required string Avatar { get; init; }
    public required string Portrait { get; init; }
    public required string Bio { get; set; }
    public required string Nameday { get; set; }

    public required ClassJob ActiveClassJob { get; init; }
    public required short ActiveClassJobLevel { get; init; }
    public required string ActiveClassJobIcon { get; init; }

    public GrandCompany GrandCompany { get; set; }
    public string? GrandCompanyRank { get; set; }

    public required string GuardianDeityName { get; set; }
    public required string GuardianDeityIcon { get; set; }

    public string? PvpTeam { get; set; }
    public required string RaceClanGender { get; set; }

    public string? TownName { get; set; }
    public string? TownIcon { get; set; }
}