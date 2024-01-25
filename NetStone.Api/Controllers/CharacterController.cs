using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Api.DTOs;
using NetStone.Api.Exceptions;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
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
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Results returned from Lodestone.</returns>
    [HttpPost("Search")]
    public async Task<ActionResult<CharacterSearchPage>> SearchAsync(CharacterSearchQuery query, int page = 1)
    {
        try
        {
            return await _characterService.SearchCharacterAsync(query, page);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    [HttpGet("{lodestoneId}")]
    public async Task<ActionResult<LodestoneCharacterDto>> GetAsync(string lodestoneId)
    {
        try
        {
            return await _characterService.GetCharacterAsync(lodestoneId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get a character's ClassJobs.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <returns>Character class jobs.</returns>
    [HttpGet("ClassJobs/{lodestoneId}")]
    public async Task<ActionResult<CharacterClassJob>> GetClassJobsAsync(string lodestoneId)
    {
        try
        {
            return await _characterService.GetCharacterClassJobsAsync(lodestoneId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /*
    [HttpGet("Achievements/{lodestoneId}")]
    public async Task<ActionResult<CharacterAchievementPage>> GetAchievementsAsync(string lodestoneId, int page = 1)
    {
        try
        {
            return await _characterService.GetCharacterAchievements(lodestoneId, page);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
    */

    /// <summary>
    ///     Get a character's minions.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <returns>Character minions.</returns>
    [HttpGet("Minions/{lodestoneId}")]
    public async Task<ActionResult<CharacterCollectable>> GetMinionsAsync(string lodestoneId)
    {
        try
        {
            return await _characterService.GetCharacterMinions(lodestoneId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get a character's mounts.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <returns>Character mounts.</returns>
    [HttpGet("Mounts/{lodestoneId}")]
    public async Task<ActionResult<CharacterCollectable>> GetMountsAsync(string lodestoneId)
    {
        try
        {
            return await _characterService.GetCharacterMounts(lodestoneId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}