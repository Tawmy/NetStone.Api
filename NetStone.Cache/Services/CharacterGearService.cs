using AutoMapper;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.Services;

public class CharacterGearService(IMapper mapper)
{
    public ICollection<CharacterGear> GetGear(Model.Parseables.Character.Gear.CharacterGear gear,
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

    private CharacterGear? ToGear(GearEntry? gearEntry, GearSlot slot, IEnumerable<CharacterGear> currentGear)
    {
        if (gearEntry is null)
        {
            return null;
        }

        var gear = mapper.Map<CharacterGear>(gearEntry);
        gear.Slot = slot;
        gear.Id = currentGear.Where(x => x.Slot == slot).Select(x => x.Id).FirstOrDefault();
        return gear;
    }

    private CharacterGear? ToGear(SoulcrystalEntry? gearEntry, IEnumerable<CharacterGear> currentGear)
    {
        if (gearEntry is null)
        {
            return null;
        }

        var gear = mapper.Map<CharacterGear>(gearEntry);
        gear.Slot = GearSlot.SoulCrystal;
        gear.Id = currentGear.Where(x => x.Slot == GearSlot.SoulCrystal).Select(x => x.Id).FirstOrDefault();
        return gear;
    }
}