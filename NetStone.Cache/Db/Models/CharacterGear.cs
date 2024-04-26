using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Models;

public class CharacterGear
{
    public int Id { get; set; } // PK

    public required int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public required GearSlot Slot { get; set; }

    public required string ItemName { get; set; }

    public string? ItemDatabaseLink { get; set; }
    public bool? IsHq { get; set; }
    public string? StrippedItemName { get; set; }
    public string? GlamourDatabaseLink { get; set; }
    public string? GlamourName { get; set; }

    public string? CreatorName { get; set; }

    public string? Materia1 { get; set; }
    public string? Materia2 { get; set; }
    public string? Materia3 { get; set; }
    public string? Materia4 { get; set; }
    public string? Materia5 { get; set; }
}