namespace NetStone.Queue.Messages;

public record GetCharacterMinionsMessage(string LodestoneId, int? MaxAge);