namespace NetStone.Common.DTOs;

public record CharacterMountOuterDto(IEnumerable<CharacterMountDto> Mounts, bool Cached, DateTime? LastUpdated);