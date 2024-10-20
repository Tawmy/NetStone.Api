namespace NetStone.Cache.Db.Models;

public class CharacterAttributes
{
    public int Id { get; set; } // PK

    public required int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public required int Strength { get; set; }
    public required int Dexterity { get; set; }
    public required int Vitality { get; set; }
    public required int Intelligence { get; set; }
    public required int Mind { get; set; }
    public required int CriticalHitRate { get; set; }
    public required int Determination { get; set; }
    public required int DirectHitRate { get; set; }
    public required int Defense { get; set; }
    public required int MagicDefense { get; set; }
    public required int AttackPower { get; set; }
    public required int SkillSpeed { get; set; }
    public int? AttackMagicPotency { get; set; }
    public int? HealingMagicPotency { get; set; }
    public int? SpellSpeed { get; set; }
    public int? Tenacity { get; set; }
    public int? Piety { get; set; }
    public int? Craftsmanship { get; set; }
    public int? Control { get; set; }
    public int? Gathering { get; set; }
    public int? Perception { get; set; }
    public required int Hp { get; set; }
    public required int MpGpCp { get; set; }
    public required string MpGpCpParameterName { get; set; }
}