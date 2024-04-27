using NetStone.Cache.Interfaces;
using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class CharacterClassJob : IUpdatable
{
    public int Id { get; set; } // PK

    public required int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public required ClassJob ClassJob { get; set; }

    public required bool IsJobUnlocked { get; init; }

    public required short Level { get; init; }

    public required int ExpCurrent { get; init; }
    public required int ExpMax { get; init; }
    public required int ExpToGo { get; init; }

    public required bool IsSpecialized { get; init; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}