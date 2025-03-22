using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterMinionMappingExtensions
{
    public static CharacterMinionDto ToDto(this CharacterMinion source)
    {
        return new CharacterMinionDto(source.Name);
    }
}