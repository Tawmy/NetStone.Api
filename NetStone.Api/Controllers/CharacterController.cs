using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Api.DTOs;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Controllers;

/// <summary>
///     Character controller. Parses Lodestone for Character data.
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    /// <inheritdoc />
    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    /// <summary>
    ///     Search for character with provided search query.
    /// </summary>
    /// <param name="query">Search query, CharacterName and World are needed.</param>
    /// <returns>Results returned from Lodestone.</returns>
    [HttpPost("Search")]
    public async Task<CharacterSearchPage?> SearchAsync(CharacterSearchQuery query)
    {
        return await _characterService.SearchCharacterAsync(query);
    }

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if it isn't known.</param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    [HttpGet("{lodestoneId}")]
    public async Task<LodestoneCharacterDto?> GetAsync(string lodestoneId)
    {
        return await _characterService.GetCharacterAsync(lodestoneId);
    }
}