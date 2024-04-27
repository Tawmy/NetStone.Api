namespace NetStone.Common.DTOs.Character;

public record CharacterMinionOuterDto(IEnumerable<CharacterMinionDto> Minions, bool Cached, DateTime? LastUpdated);