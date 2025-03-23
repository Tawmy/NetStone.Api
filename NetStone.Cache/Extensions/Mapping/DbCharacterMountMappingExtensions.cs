using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterMountMappingExtensions
{
    public static CharacterMountDto ToDto(this CharacterMount source)
    {
        return new CharacterMountDto(source.Name);
    }
}