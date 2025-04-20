using System.Diagnostics;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneSoulcrystalEntryMappingExtension
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneSoulcrystalEntryMappingExtension));

    /// <remarks>Attach this directly to the character so FK is set by EF</remarks>
    public static CharacterGear ToDb(this SoulcrystalEntry source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterGear
        {
            CharacterId = 0, // set automatically by EF

            Slot = GearSlot.SoulCrystal,

            ItemName = source.ItemName,

            // special case, usually gear must have level. crystal level 0 is the compromise to keep it non-nullable
            ItemLevel = 0
        };
    }
}