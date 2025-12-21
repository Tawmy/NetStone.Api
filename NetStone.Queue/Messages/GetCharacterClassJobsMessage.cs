using NetStone.Common.Enums;

namespace NetStone.Queue.Messages;

public record GetCharacterClassJobsMessage(string LodestoneId, int? MaxAge, FallbackTypeV4? UseFallback);