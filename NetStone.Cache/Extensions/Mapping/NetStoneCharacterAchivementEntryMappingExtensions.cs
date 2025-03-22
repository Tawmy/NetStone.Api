using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterAchivementEntryMappingExtensions
{
    public static CharacterAchievement ToDb(this CharacterAchievementEntry source, string characterLodestoneId)
    {
        return new CharacterAchievement
        {
            CharacterLodestoneId = characterLodestoneId,
            AchievementId = source.Id ?? throw new InvalidOperationException($"{nameof(source.Id)} must not be null"),
            Name = source.Name,
            DatabaseLink = source.DatabaseLink ??
                           throw new InvalidOperationException($"{nameof(source.DatabaseLink)} must not be null"),
            TimeAchieved = source.TimeAchieved
        };
    }
}