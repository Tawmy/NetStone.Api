using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class Character
{
    public int Id { get; set; } //PK

    public required string LodestoneId { get; set; }

    public required ClassJob ActiveClassJob { get; set; }
    public required short ActiveClassJobLevel { get; set; }
    public required string ActiveClassJobIcon { get; set; }

    public required string Avatar { get; set; }
    public required string Bio { get; set; }

    public CharacterFreeCompany? FreeCompany { get; set; }

    public int? FullFreeCompanyId { get; set; }
    public FreeCompany? FullFreeCompany { get; set; }

    public GrandCompany GrandCompany { get; set; }
    public string? GrandCompanyRank { get; set; }

    public required string GuardianDeityName { get; set; }
    public required string GuardianDeityIcon { get; set; }

    public required string Name { get; set; }
    public required string Nameday { get; set; }

    public required string Portrait { get; set; }

    public string? PvpTeam { get; set; }

    public required Race Race { get; set; }
    public required Tribe Tribe { get; set; }
    public required Gender Gender { get; set; }

    public required string Server { get; set; }

    public string? Title { get; set; }

    public string? TownName { get; set; }
    public string? TownIcon { get; set; }

    public ICollection<CharacterGear> Gear { get; set; } = new HashSet<CharacterGear>();

    public CharacterAttributes Attributes { get; set; } = null!; // TODO complex type?

    public ICollection<CharacterClassJob> CharacterClassJobs { get; set; } = new HashSet<CharacterClassJob>();

    public ICollection<CharacterMinion> Minions { get; set; } = new HashSet<CharacterMinion>();

    public ICollection<CharacterMount> Mounts { get; set; } = new HashSet<CharacterMount>();

    public ICollection<CharacterAchievement> Achievements { get; set; } = new HashSet<CharacterAchievement>();

    public FreeCompanyMember? FreeCompanyMembership { get; set; } // TODO complex type?

    public DateTime CharacterUpdatedAt { get; set; }
    public DateTime? CharacterClassJobsUpdatedAt { get; set; }
    public DateTime? CharacterMinionsUpdatedAt { get; set; }
    public DateTime? CharacterMountsUpdatedAt { get; set; }
    public DateTime? CharacterAchievementsUpdatedAt { get; set; }
}