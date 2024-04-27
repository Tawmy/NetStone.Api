using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.ClassJob;

namespace NetStone.Cache.Interfaces;

public interface ICharacterCachingService
{
    Task<CharacterDto> CacheCharacterAsync(string lodestoneId, LodestoneCharacter character);
    Task<CharacterDto?> GetCharacterAsync(int id);
    Task<CharacterDto?> GetCharacterAsync(string lodestoneId);

    Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(int id);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId);
}