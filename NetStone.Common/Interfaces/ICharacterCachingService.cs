using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character;

namespace NetStone.Common.Interfaces;

public interface ICharacterCachingService
{
    Task CacheCharacterAsync(LodestoneCharacter character, string lodestoneId);
    Task<CharacterDto?> GetCharacterAsync(int id);
    Task<CharacterDto?> GetCharacterAsync(string lodestoneId);
}