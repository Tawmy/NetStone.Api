using NetStone.Common.DTOs.Character;

namespace NetStone.Common.DTOs.FreeCompany;

public record FreeCompanyMemberDto
{
    public required string LodestoneId { get; init; }

    public required string FreeCompanyLodestoneId { get; init; }

    public CharacterDto? CachedCharacter { get; init; }

    public required string Name { get; init; }
    public string? Rank { get; init; }
    public string? RankIcon { get; init; }
    public required string Server { get; init; }
    public required string DataCenter { get; init; }
    public required string Avatar { get; init; }
}