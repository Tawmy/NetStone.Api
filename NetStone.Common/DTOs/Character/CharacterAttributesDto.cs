namespace NetStone.Common.DTOs.Character;

public record CharacterAttributesDto
{
    public required int Strength { get; init; }
    public required int Dexterity { get; init; }
    public required int Vitality { get; init; }
    public required int Intelligence { get; init; }
    public required int Mind { get; init; }
    public required int CriticalHitRate { get; init; }
    public required int Determination { get; init; }
    public required int DirectHitRate { get; init; }
    public required int Defense { get; init; }
    public required int MagicDefense { get; init; }
    public required int AttackPower { get; init; }
    public required int SkillSpeed { get; init; }
    public required int AttackMagicPotency { get; init; }
    public required int HealingMagicPotency { get; init; }
    public int? SpellSpeed { get; init; }
    public int? Tenacity { get; init; }
    public int? Piety { get; init; }
    public required int Hp { get; init; }
    public required int MpGpCp { get; init; }
    public required string MpGpCpParameterName { get; init; }
}