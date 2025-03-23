using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.Interfaces;

public interface ICharacterCachingServiceV3
{
    #region Character

    Task<CharacterDtoV3> CacheCharacterAsync(string lodestoneId, LodestoneCharacter character);
    Task<CharacterDtoV3?> GetCharacterAsync(int id);
    Task<CharacterDtoV3?> GetCharacterAsync(string lodestoneId);

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

    #region CharacterAchievements

    Task<ICollection<CharacterAchievementDto>> CacheCharacterAchievementsAsync(string lodestoneId,
        IEnumerable<CharacterAchievementEntry> lodestoneAchievements);

    Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(int id);

    Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(
        string lodestoneId);

    #endregion
}