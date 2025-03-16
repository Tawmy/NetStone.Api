using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;

namespace NetStone.Cache.Db.Resolvers;

public class CharacterAttributeResolver
    : IValueResolver<Character, CharacterDtoV3, IDictionary<CharacterAttribute, int?>>
{
    public IDictionary<CharacterAttribute, int?> Resolve(Character source, CharacterDtoV3 destination,
        IDictionary<CharacterAttribute, int?> destMember, ResolutionContext context)
    {
        var dict = new Dictionary<CharacterAttribute, int?>
        {
            { CharacterAttribute.Hp, source.Attributes.Hp },

            { CharacterAttribute.Strength, source.Attributes.Strength },
            { CharacterAttribute.Dexterity, source.Attributes.Dexterity },
            { CharacterAttribute.Vitality, source.Attributes.Vitality },
            { CharacterAttribute.Intelligence, source.Attributes.Intelligence },
            { CharacterAttribute.Mind, source.Attributes.Mind },

            { CharacterAttribute.CriticalHitRate, source.Attributes.CriticalHitRate },
            { CharacterAttribute.Determination, source.Attributes.Determination },
            { CharacterAttribute.DirectHitRate, source.Attributes.DirectHitRate },

            { CharacterAttribute.Defense, source.Attributes.Defense },
            { CharacterAttribute.MagicDefense, source.Attributes.MagicDefense },

            { CharacterAttribute.AttackPower, source.Attributes.AttackPower },
            { CharacterAttribute.SkillSpeed, source.Attributes.SkillSpeed }
        };

        if (source.ActiveClassJob.IsDiscipleOfHand())
        {
            dict.Add(CharacterAttribute.Cp, source.Attributes.MpGpCp);

            dict.Add(CharacterAttribute.Craftsmanship, source.Attributes.Craftsmanship);
            dict.Add(CharacterAttribute.Control, source.Attributes.Control);
        }
        else if (source.ActiveClassJob.IsDiscipleOfLand())
        {
            dict.Add(CharacterAttribute.Gp, source.Attributes.MpGpCp);

            dict.Add(CharacterAttribute.Gathering, source.Attributes.Gathering);
            dict.Add(CharacterAttribute.Perception, source.Attributes.Perception);
        }
        else
        {
            dict.Add(CharacterAttribute.Mp, source.Attributes.MpGpCp);

            dict.Add(CharacterAttribute.AttackMagicPotency, source.Attributes.AttackMagicPotency);
            dict.Add(CharacterAttribute.HealingMagicPotency, source.Attributes.HealingMagicPotency);
            dict.Add(CharacterAttribute.SpellSpeed, source.Attributes.SpellSpeed);

            dict.Add(CharacterAttribute.Tenacity, source.Attributes.Tenacity);
            dict.Add(CharacterAttribute.Piety, source.Attributes.Piety);
        }

        return dict;
    }
}