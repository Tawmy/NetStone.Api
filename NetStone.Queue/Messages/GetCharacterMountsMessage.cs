using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterMountsMessage(string LodestoneId, int? MaxAge, FallbackTypeV4? UseFallback);