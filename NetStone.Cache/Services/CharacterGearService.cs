using AspNetCoreExtensions;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.Services;

// Can currently be static, but might need DI later
public static class CharacterGearService
{
    public static ICollection<CharacterGear> GetGear(Model.Parseables.Character.Gear.CharacterGear gear,
        ICollection<CharacterGear> currentGear)
    {
        var list = new List<CharacterGear>
        {
            ToGear(gear.Mainhand, GearSlot.MainHand, currentGear)! // must be not null
        };

        list.AddIfNotNull(ToGear(gear.Offhand, GearSlot.OffHand, currentGear));
        list.AddIfNotNull(ToGear(gear.Head, GearSlot.Head, currentGear));
        list.AddIfNotNull(ToGear(gear.Body, GearSlot.Body, currentGear));
        list.AddIfNotNull(ToGear(gear.Hands, GearSlot.Hands, currentGear));
        list.AddIfNotNull(ToGear(gear.Legs, GearSlot.Legs, currentGear));
        list.AddIfNotNull(ToGear(gear.Feet, GearSlot.Feet, currentGear));
        list.AddIfNotNull(ToGear(gear.Earrings, GearSlot.Earrings, currentGear));
        list.AddIfNotNull(ToGear(gear.Necklace, GearSlot.Necklace, currentGear));
        list.AddIfNotNull(ToGear(gear.Bracelets, GearSlot.Bracelets, currentGear));
        list.AddIfNotNull(ToGear(gear.Ring1, GearSlot.Ring1, currentGear));
        list.AddIfNotNull(ToGear(gear.Ring2, GearSlot.Ring2, currentGear));
        list.AddIfNotNull(ToGear(gear.Soulcrystal, currentGear));

        return list;
    }

    private static CharacterGear? ToGear(GearEntry? gearEntry, GearSlot slot, IEnumerable<CharacterGear> currentGear)
    {
        if (gearEntry is null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(gearEntry.Dye1Name))
        {
            if (string.IsNullOrEmpty(gearEntry.Dye1Color))
            {
                throw new InvalidOperationException("Dye 1 name was parsed, but dye 1 color failed.");
            }

            if (gearEntry.Dye1DatabaseLink is null)
            {
                throw new InvalidOperationException("Dye 1 name was parsed, but dye 1 database link failed.");
            }
        }

        if (!string.IsNullOrEmpty(gearEntry.Dye2Name))
        {
            if (string.IsNullOrEmpty(gearEntry.Dye2Color))
            {
                throw new InvalidOperationException("Dye 2 name was parsed, but dye 2 color failed.");
            }

            if (gearEntry.Dye2DatabaseLink is null)
            {
                throw new InvalidOperationException("Dye 2 name was parsed, but dye 2 database link failed.");
            }
        }

        var gear = gearEntry.ToDb(slot);
        gear.Id = currentGear.Where(x => x.Slot == slot).Select(x => x.Id).FirstOrDefault();
        return gear;
    }

    private static CharacterGear? ToGear(SoulcrystalEntry? gearEntry, IEnumerable<CharacterGear> currentGear)
    {
        if (gearEntry is null)
        {
            return null;
        }

        var gear = gearEntry.ToDb();
        gear.Id = currentGear.Where(x => x.Slot is GearSlot.SoulCrystal).Select(x => x.Id).FirstOrDefault();
        return gear;
    }
}