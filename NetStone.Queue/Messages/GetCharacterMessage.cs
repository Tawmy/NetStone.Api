using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterMessage(string LodestoneId, int? MaxAge, FallbackType? UseFallback);