using System.Diagnostics;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneGearEntryMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneGearEntryMappingExtensions));

    /// <remarks>Attach this directly to the character so FK is set by EF</remarks>
    public static CharacterGear ToDb(this GearEntry source, GearSlot slot)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterGear
        {
            CharacterId = 0, // set automatically by EF

            Slot = slot,

            ItemName = source.ItemName,
            ItemLevel = source.ItemLevel,

            ItemDatabaseLink = source.ItemDatabaseLink?.ToString() ??
                               throw new InvalidOperationException(
                                   $"{nameof(source.ItemDatabaseLink)} must not be null"),
            IsHq = source.IsHq,
            StrippedItemName = source.StrippedItemName,
            GlamourDatabaseLink = source.GlamourDatabaseLink?.ToString(),
            GlamourName = source.GlamourName,
            CreatorName = source.CreatorName,
            Materia1 = source.Materia.ElementAtOrDefault(0),
            Materia2 = source.Materia.ElementAtOrDefault(1),
            Materia3 = source.Materia.ElementAtOrDefault(2),
            Materia4 = source.Materia.ElementAtOrDefault(3),
            Materia5 = source.Materia.ElementAtOrDefault(4)
        };
    }
}