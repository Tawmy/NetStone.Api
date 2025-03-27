using System.Diagnostics;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterAttributesMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneCharacterAttributesMappingExtensions));

    /// <remarks>Attach these directly to the character so FK is set by EF</remarks>
    public static CharacterAttributes ToDb(this Model.Parseables.Character.CharacterAttributes source, int? id = null)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterAttributes
        {
            Id = id ?? 0, // set automatically by EF
            CharacterId = 0, // set automatically by EF

            Strength = source.Strength,
            Dexterity = source.Dexterity,
            Vitality = source.Vitality,
            Intelligence = source.Intelligence,
            Mind = source.Mind,
            CriticalHitRate = source.CriticalHitRate,
            Determination = source.Determination,
            DirectHitRate = source.DirectHitRate,
            Defense = source.Defense,
            MagicDefense = source.MagicDefense,
            AttackPower = source.AttackPower,
            SkillSpeed = source.SkillSpeed,
            AttackMagicPotency = source.AttackMagicPotency,
            HealingMagicPotency = source.HealingMagicPotency,
            SpellSpeed = source.SpellSpeed,
            Tenacity = source.Tenacity,
            Piety = source.Piety,
            Craftsmanship = source.Craftsmanship,
            Control = source.Control,
            Gathering = source.Gathering,
            Perception = source.Perception,
            Hp = source.Hp,
            MpGpCp = source.MpGpCp,
            MpGpCpParameterName = source.MpGpCpParameterName
        };
    }
}