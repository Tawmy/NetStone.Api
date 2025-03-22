using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Search.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterSearchPageMappingExtensions
{
    public static CharacterSearchPageDto ToDto(this CharacterSearchPage source)
    {
        return new CharacterSearchPageDto(source.HasResults, source.Results.Select(x => x.ToDto()), source.CurrentPage,
            source.NumPages);
    }
}