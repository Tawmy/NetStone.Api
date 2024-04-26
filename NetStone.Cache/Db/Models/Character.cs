using NetStone.Cache.Interfaces;
using NetStone.StaticData;

namespace NetStone.Cache.Db.Models;

public class Character : IUpdatable
{
    public int Id { get; set; } //PK

    public required string LodestoneId { get; set; }

    public required ClassJob ActiveClassJob { get; set; }
    public required short ActiveClassJobLevel { get; set; }
    public required string ActiveClassJobIcon { get; set; }

    public required string Avatar { get; set; }
    public required string Bio { get; set; }

    public CharacterFreeCompany? FreeCompany { get; set; }

    public GrandCompany GrandCompany { get; set; }
    public string? GrandCompanyRank { get; set; }

    public required string GuardianDeityName { get; set; }
    public required string GuardianDeityIcon { get; set; }

    public required string Name { get; set; }
    public required string Nameday { get; set; }

    public required string Portrait { get; set; }

    public string? PvpTeam { get; set; }

    public required string RaceClanGender { get; set; }

    public required string Server { get; set; }

    public string? Title { get; set; }

    public string? TownName { get; set; }
    public string? TownIcon { get; set; }

    public ICollection<CharacterGear> Gear { get; set; } = new HashSet<CharacterGear>();

    public CharacterAttributes Attributes { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}