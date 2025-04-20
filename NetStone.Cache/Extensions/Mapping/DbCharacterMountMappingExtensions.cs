using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterMountMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterMountMappingExtensions));

    public static CharacterMountDto ToDto(this CharacterMount source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterMountDto(source.Name);
    }
}