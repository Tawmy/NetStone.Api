namespace NetStone.Queue.Messages;

public record GetCharacterAchievementsMessage(string LodestoneId, int? MaxAge, bool? UseFallback);