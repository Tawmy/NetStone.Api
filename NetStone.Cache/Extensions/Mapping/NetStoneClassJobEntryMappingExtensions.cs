using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character.ClassJob;
using CharacterClassJob = NetStone.Cache.Db.Models.CharacterClassJob;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneClassJobEntryMappingExtensions
{
    public static CharacterClassJob ToDb(this ClassJobEntry source, string characterLodestoneId, ClassJob classJob)
    {
        return new CharacterClassJob
        {
            CharacterLodestoneId = characterLodestoneId,
            ClassJob = classJob,
            IsJobUnlocked = source.IsJobUnlocked,
            Level = (short)source.Level,
            ExpCurrent = source.ExpCurrent,
            ExpMax = source.ExpMax,
            ExpToGo = source.ExpToGo,
            IsSpecialized = source.IsSpecialized
        };
    }
}