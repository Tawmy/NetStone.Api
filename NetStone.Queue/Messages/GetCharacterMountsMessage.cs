namespace NetStone.Queue.Messages;

public record GetCharacterMountsMessage(string LodestoneId, int? MaxAge);