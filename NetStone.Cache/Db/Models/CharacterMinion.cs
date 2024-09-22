namespace NetStone.Cache.Db.Models;

public class CharacterMinion
{
    public int Id { get; set; } // PK

    public required string CharacterLodestoneId { get; set; }

    public int? CharacterId { get; set; }
    public Character? Character { get; set; }

    public required string Name { get; set; }
}