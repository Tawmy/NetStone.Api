using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.Interfaces;

public interface ICharacterCachingService
{
    #region Character

    Task<CharacterDto> CacheCharacterAsync(string lodestoneId, LodestoneCharacter character);
    Task<CharacterDto?> GetCharacterAsync(int id);
    Task<CharacterDto?> GetCharacterAsync(string lodestoneId);

    #endregion

    #region CharacterClassJobs

    Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(int id);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId);

    #endregion

    #region CharacterMinions

    Task<ICollection<CharacterMinionDto>> CacheCharacterMinionsAsync(string lodestoneId,
        CharacterCollectable lodestoneMinions);

    Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(int id);
    Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(string lodestoneId);

    #endregion

    #region CharacterMounts

    Task<ICollection<CharacterMountDto>> CacheCharacterMountsAsync(string lodestoneId,
        CharacterCollectable lodestoneMounts);

    Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(int id);
    Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(string lodestoneId);

    #endregion
}