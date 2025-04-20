using System.Diagnostics;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Search.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterSearchPageMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneCharacterSearchPageMappingExtensions));

    public static CharacterSearchPageDto ToDto(this CharacterSearchPage source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterSearchPageDto(source.HasResults, source.Results.Select(x => x.ToDto()), source.CurrentPage,
            source.NumPages);
    }
}