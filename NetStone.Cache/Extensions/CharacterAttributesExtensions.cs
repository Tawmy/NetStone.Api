using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;

namespace NetStone.Cache.Extensions;

internal static class CharacterAttributesExtensions
{
    public static IDictionary<CharacterAttribute, int?> ToDictionary(this CharacterAttributes attributes,
        ClassJob activeClassJob)
    {
        var dict = new Dictionary<CharacterAttribute, int?>
        {
            { CharacterAttribute.Hp, attributes.Hp },

            { CharacterAttribute.Strength, attributes.Strength },
            { CharacterAttribute.Dexterity, attributes.Dexterity },
            { CharacterAttribute.Vitality, attributes.Vitality },
            { CharacterAttribute.Intelligence, attributes.Intelligence },
            { CharacterAttribute.Mind, attributes.Mind },

            { CharacterAttribute.CriticalHitRate, attributes.CriticalHitRate },
            { CharacterAttribute.Determination, attributes.Determination },
            { CharacterAttribute.DirectHitRate, attributes.DirectHitRate },

            { CharacterAttribute.Defense, attributes.Defense },
            { CharacterAttribute.MagicDefense, attributes.MagicDefense },

            { CharacterAttribute.AttackPower, attributes.AttackPower },
            { CharacterAttribute.SkillSpeed, attributes.SkillSpeed }
        };

        if (activeClassJob.IsDiscipleOfHand())
        {
            dict.Add(CharacterAttribute.Cp, attributes.MpGpCp);

            dict.Add(CharacterAttribute.Craftsmanship, attributes.Craftsmanship);
            dict.Add(CharacterAttribute.Control, attributes.Control);
        }
        else if (activeClassJob.IsDiscipleOfLand())
        {
            dict.Add(CharacterAttribute.Gp, attributes.MpGpCp);

            dict.Add(CharacterAttribute.Gathering, attributes.Gathering);
            dict.Add(CharacterAttribute.Perception, attributes.Perception);
        }
        else
        {
            dict.Add(CharacterAttribute.Mp, attributes.MpGpCp);

            dict.Add(CharacterAttribute.AttackMagicPotency, attributes.AttackMagicPotency);
            dict.Add(CharacterAttribute.HealingMagicPotency, attributes.HealingMagicPotency);
            dict.Add(CharacterAttribute.SpellSpeed, attributes.SpellSpeed);

            dict.Add(CharacterAttribute.Tenacity, attributes.Tenacity);
            dict.Add(CharacterAttribute.Piety, attributes.Piety);
        }

        return dict;
    }
}