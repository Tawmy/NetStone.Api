using System.Diagnostics;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Search.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterSearchEntryMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneCharacterSearchEntryMappingExtensions));

    public static CharacterSearchPageResultDto ToDto(this CharacterSearchEntry source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterSearchPageResultDto(source.Name,
            source.Id ?? throw new InvalidOperationException($"{nameof(source.Id)} must not be null."));
    }
}