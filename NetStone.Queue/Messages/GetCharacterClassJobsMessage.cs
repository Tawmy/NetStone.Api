namespace NetStone.Queue.Messages;

public record GetCharacterClassJobsMessage(string LodestoneId, int? MaxAge);