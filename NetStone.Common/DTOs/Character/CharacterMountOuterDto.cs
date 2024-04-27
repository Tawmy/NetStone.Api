namespace NetStone.Common.DTOs.Character;

public record CharacterMountOuterDto(IEnumerable<CharacterMountDto> Mounts, bool Cached, DateTime? LastUpdated);