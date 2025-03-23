using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Search.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterSearchEntryMappingExtensions
{
    public static CharacterSearchPageResultDto ToDto(this CharacterSearchEntry source)
    {
        return new CharacterSearchPageResultDto(source.Name,
            source.Id ?? throw new InvalidOperationException($"{nameof(source.Id)} must not be null."));
    }
}