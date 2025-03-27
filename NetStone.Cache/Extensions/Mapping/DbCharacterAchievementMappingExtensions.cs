using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterAchievementMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterAchievementMappingExtensions));

    public static CharacterAchievementDto ToDto(this CharacterAchievement source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterAchievementDto
        {
            Id = source.AchievementId,
            Name = source.Name,
            DatabaseLink = source.DatabaseLink,
            TimeAchieved = source.TimeAchieved
        };
    }
}