using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class CharacterClassJob
{
    public int Id { get; set; } // PK

    public required string CharacterLodestoneId { get; set; }

    public int? CharacterId { get; set; }
    public Character? Character { get; set; }

    public required ClassJob ClassJob { get; set; }

    public required bool IsJobUnlocked { get; init; }

    public required short Level { get; init; }

    public required int ExpCurrent { get; init; }
    public required int ExpMax { get; init; }
    public required int ExpToGo { get; init; }

    public required bool IsSpecialized { get; init; }
}