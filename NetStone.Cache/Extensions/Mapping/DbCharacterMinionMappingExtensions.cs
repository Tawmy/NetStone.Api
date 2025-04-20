using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterMinionMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterMinionMappingExtensions));

    public static CharacterMinionDto ToDto(this CharacterMinion source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterMinionDto(source.Name);
    }
}