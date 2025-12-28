using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.Interfaces;

public interface ICharacterCachingService
{
    #region Character

    Task<CharacterDto> CacheCharacterAsync(string lodestoneId, LodestoneCharacter character, bool cacheImages,
        CT ct = default);

    Task<CharacterDto?> GetCharacterAsync(int id, CT ct = default);
    Task<CharacterDto?> GetCharacterAsync(string lodestoneId, CT ct = default);
    Task<CharacterDto?> GetCharacterAsync(string name, string world, CT ct = default);

    #endregion

    #region CharacterClassJobs

    Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs, CT ct = default);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(int id,
        CT ct = default);

    Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId, CT ct = default);

    #endregion

    #region CharacterMinions

    Task<ICollection<CharacterMinionDto>> CacheCharacterMinionsAsync(string lodestoneId,
        CharacterCollectable lodestoneMinions, CT ct = default);

    Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(int id, CT ct = default);

    Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(string lodestoneId,
        CT ct = default);

    #endregion

    #region CharacterMounts

    Task<ICollection<CharacterMountDto>> CacheCharacterMountsAsync(string lodestoneId,
        CharacterCollectable lodestoneMounts, CT ct = default);

    Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(int id, CT ct = default);

    Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(string lodestoneId,
        CT ct = default);

    #endregion

    #region CharacterAchievements

    Task<ICollection<CharacterAchievementDto>> CacheCharacterAchievementsAsync(string lodestoneId,
        IEnumerable<CharacterAchievementEntry> lodestoneAchievements, CT ct = default);

    Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(int id,
        CT ct = default);

    Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(
        string lodestoneId, CT ct = default);

    #endregion
}