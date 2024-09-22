namespace NetStone.Cache.Db.Models;

public class CharacterAchievement
{
    public int Id { get; set; } // PK

    public required string CharacterLodestoneId { get; set; }

    public int? CharacterId { get; set; }
    public Character? Character { get; set; }

    public required ulong AchievementId { get; set; }
    public required string Name { get; set; }
    public required Uri DatabaseLink { get; set; }
    public required DateTime TimeAchieved { get; set; }
}