namespace NetStone.Cache.Db.Models;

public class CharacterFreeCompany
{
    public int Id { get; set; } // PK

    public required int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public required string LodestoneId { get; set; }

    public required string Name { get; set; }
    public required string Link { get; set; }

    public string? TopLayer { get; set; }
    public string? MiddleLayer { get; set; }
    public string? BottomLayer { get; set; }
}