using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class CharacterClassJob
{
    public int Id { get; set; } // PK

    public required string CharacterLodestoneId { get; set; }

    public int? CharacterId { get; set; }
    public Character? Character { get; set; }

    public required ClassJob ClassJob { get; set; }

    public required bool IsJobUnlocked { get; set; }

    public required short Level { get; set; }

    public required long ExpCurrent { get; set; }
    public required long ExpMax { get; set; }
    public required long ExpToGo { get; set; }

    public required bool IsSpecialized { get; set; }
}