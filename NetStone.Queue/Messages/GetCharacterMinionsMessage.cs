using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterMinionsMessage(string LodestoneId, int? MaxAge, FallbackTypeV4? UseFallback);