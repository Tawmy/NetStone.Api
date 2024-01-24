using NetStone.Api.Controllers;
using NetStone.Api.DTOs;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Interfaces;

/// <summary>
///     Controller service for <see cref="CharacterController" />.
/// </summary>
public interface ICharacterService
{
    /// <summary>
    ///     Search for character with provided search query.
    /// </summary>
    /// <param name="query">
    ///     Search query, only <see cref="CharacterSearchQuery.CharacterName" /> and
    ///     <see cref="CharacterSearchQuery.World" /> are needed.
    /// </param>
    /// <returns>Instance of <see cref="CharacterSearchPage" /> with the results returned from the Lodestone.</returns>
    public Task<CharacterSearchPage?> SearchCharacterAsync(CharacterSearchQuery query);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchCharacterAsync" /> first if it isn't known.</param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    public Task<LodestoneCharacterDto?> GetCharacterAsync(string lodestoneId);
}