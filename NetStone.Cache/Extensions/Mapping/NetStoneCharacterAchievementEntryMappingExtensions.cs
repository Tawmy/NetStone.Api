using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterAchievementEntryMappingExtensions
{
    private static readonly ActivitySource ActivitySource =
        new(nameof(NetStoneCharacterAchievementEntryMappingExtensions));

    public static CharacterAchievement ToDb(this CharacterAchievementEntry source, string characterLodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

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