namespace NetStone.Common.DTOs;

public record CharacterMinionOuterDto(IEnumerable<CharacterMinionDto> Minions, bool Cached, DateTime? LastUpdated);