namespace NetStone.Queue.Messages;

public record GetFreeCompanyMessage(string LodestoneId, int? MaxAge);