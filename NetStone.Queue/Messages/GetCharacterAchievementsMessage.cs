using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterAchievementsMessage(string LodestoneId, int? MaxAge, FallbackTypeV4? UseFallback);