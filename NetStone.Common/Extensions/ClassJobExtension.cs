using NetStone.Common.Enums;

namespace NetStone.Common.Extensions;

public static class ClassJobExtension
{
    public static bool EvolvesFromClass(this ClassJob job)
    {
        return job switch
        {
            ClassJob.Paladin or ClassJob.Warrior or ClassJob.Monk or ClassJob.Dragoon or ClassJob.Bard
                or ClassJob.WhiteMage or ClassJob.BlackMage or ClassJob.Summoner or ClassJob.Scholar
                or ClassJob.Ninja => true,
            _ => false
        };
    }
}