using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterClassJobMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterClassJobMappingExtensions));

    public static void ToDb(this CharacterClassJob source, CharacterClassJob target)
    {
        using var activity = ActivitySource.StartActivity();

        target.ClassJob = source.ClassJob;
        target.IsJobUnlocked = source.IsJobUnlocked;
        target.Level = source.Level;
        target.ExpCurrent = source.ExpCurrent;
        target.ExpMax = source.ExpMax;
        target.ExpToGo = source.ExpToGo;
        target.IsSpecialized = source.IsSpecialized;
    }

    public static CharacterClassJobDto ToDto(this CharacterClassJob source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterClassJobDto
        {
            ClassJob = source.ClassJob,
            IsJobUnlocked = source.IsJobUnlocked,
            Level = source.Level,
            ExpCurrent = source.ExpCurrent,
            ExpMax = source.ExpMax,
            ExpToGo = source.ExpToGo,
            IsSpecialized = source.IsSpecialized
        };
    }
}