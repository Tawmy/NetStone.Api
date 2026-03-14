using System.Diagnostics;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneFacewearEntryMappingExtension
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneFacewearEntryMappingExtension));

    /// <remarks>Attach this directly to the character so FK is set by EF</remarks>
    public static CharacterGear ToDb(this FacewearEntry source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterGear
        {
            CharacterId = 0, // set automatically by EF

            Slot = GearSlot.Facewear,

            ItemName = source.ItemName,

            // special case, usually gear must have level. facewear level 0 is the compromise to keep it non-nullable
            ItemLevel = 0,

            // The naming of these properties is misleading, should be separate type of item
            // TODO API V5: Turn facewear and soulcrystal into their own entities instead of shoehorning into GearEntry
            ItemDatabaseLink = source.DbLink?.ToString(),
            ItemIconLink = source.IconLink?.ToString() ??
                           throw new InvalidOperationException($"{nameof(source.IconLink)} must not be null"),
            GlamourName = source.UnlockedBy,
            GlamourIconLink = source.UnlockedByIconLink?.ToString()
        };
    }
}