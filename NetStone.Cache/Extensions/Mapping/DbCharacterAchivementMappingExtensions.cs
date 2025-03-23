using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterAchivementMappingExtensions
{
    public static CharacterAchievementDto ToDto(this CharacterAchievement source)
    {
        return new CharacterAchievementDto
        {
            Id = source.AchievementId,
            Name = source.Name,
            DatabaseLink = source.DatabaseLink,
            TimeAchieved = source.TimeAchieved
        };
    }
}