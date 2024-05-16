namespace NetStone.Cache.Db.Models;

public class FreeCompanyMember
{
    public int Id { get; set; } // PK

    public required string CharacterLodestoneId { get; set; }

    public int? FullCharacterId { get; set; }
    public Character? FullCharacter { get; set; }

    public required string FreeCompanyLodestoneId { get; set; }

    public int? FreeCompanyId { get; set; }
    public FreeCompany? FreeCompany { get; set; }

    public required string Name { get; set; }
    public string? Rank { get; set; }
    public string? RankIcon { get; set; }
    public required string Server { get; set; }
    public required string DataCenter { get; set; }
    public required string Avatar { get; set; }
}