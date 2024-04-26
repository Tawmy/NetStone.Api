using NetStone.Common.Enums;

namespace NetStone.Common.DTOs;

public record CharacterGearDto
{
    public required GearSlot Slot { get; set; }

    public required string ItemName { get; set; }

    public string? ItemDatabaseLink { get; set; }
    public bool? IsHq { get; set; }
    public string? StrippedItemName { get; set; }
    public string? GlamourDatabaseLink { get; set; }
    public string? GlamourName { get; set; }

    public string? CreatorName { get; set; }

    public IEnumerable<string> Materia { get; set; } = [];
}