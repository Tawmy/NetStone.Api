namespace NetStone.Cache.Db.Models;

public class CharacterFreeCompany
{
    public int Id { get; set; } // PK

    public required int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public required string LodestoneId { get; set; }

    public required string Name { get; set; }
    public required string Link { get; set; }

    public required string TopLayer { get; set; }
    public required string MiddleLayer { get; set; }
    public required string BottomLayer { get; set; }
}