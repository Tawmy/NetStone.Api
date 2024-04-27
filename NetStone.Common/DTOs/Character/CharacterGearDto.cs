using NetStone.Common.Enums;

namespace NetStone.Common.DTOs.Character;

public record CharacterGearDto
{
    public required GearSlot Slot { get; init; }

    public required string ItemName { get; init; }

    public string? ItemDatabaseLink { get; init; }
    public bool? IsHq { get; init; }
    public string? StrippedItemName { get; init; }
    public string? GlamourDatabaseLink { get; init; }
    public string? GlamourName { get; init; }

    public string? CreatorName { get; init; }

    public IEnumerable<string> Materia { get; init; } = [];
}