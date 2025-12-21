using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterMessage(string LodestoneId, int? MaxAge, FallbackTypeV4? UseFallback);