namespace NetStone.Queue.Messages;

public record GetFreeCompanyMembersMessage(string LodestoneId, int? MaxAge, bool? UseFallback);