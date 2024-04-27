using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Api.Interfaces;
using NetStone.Common.DTOs;
using NetStone.Common.Exceptions;
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
    /// <param name="maxAge">
    ///     Optional maximum age of cached character, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    [HttpGet("{lodestoneId}")]
    public async Task<ActionResult<CharacterDto>> GetAsync(string lodestoneId, int? maxAge)
    {
        try
        {
            return await _characterService.GetCharacterAsync(lodestoneId, maxAge);
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
    /// <param name="maxAge">
    ///     Optional maximum age of cached class jobs, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CharacterClassJobOuterDto.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <see cref="maxAge" /> is set to ANY value, the
    ///     data will be refreshed. If Character was cached at least once and the value can be saved, <see cref="maxAge" />
    ///     applies as expected.
    /// </remarks>
    /// <returns>Character class jobs.</returns>
    [HttpGet("ClassJobs/{lodestoneId}")]
    public async Task<ActionResult<CharacterClassJobOuterDto>> GetClassJobsAsync(string lodestoneId, int? maxAge)
    {
        try
        {
            return await _characterService.GetCharacterClassJobsAsync(lodestoneId, maxAge);
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
    /// <param name="maxAge">
    ///     Optional maximum age of cached minions, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CharacterMinionOuterDto.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <see cref="maxAge" /> is set to ANY value, the
    ///     data will be refreshed. If Character was cached at least once and the value can be saved, <see cref="maxAge" />
    ///     applies as expected.
    /// </remarks>
    /// <returns>Character minions.</returns>
    [HttpGet("Minions/{lodestoneId}")]
    public async Task<ActionResult<CharacterMinionOuterDto>> GetMinionsAsync(string lodestoneId, int? maxAge)
    {
        try
        {
            return await _characterService.GetCharacterMinions(lodestoneId, maxAge);
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
    /// <param name="maxAge">
    ///     Optional maximum age of cached mounts, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CharacterMountOuterDto.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <see cref="maxAge" /> is set to ANY value, the
    ///     data will be refreshed. If Character was cached at least once and the value can be saved, <see cref="maxAge" />
    ///     applies as expected.
    /// </remarks>
    /// <returns>Character mounts.</returns>
    [HttpGet("Mounts/{lodestoneId}")]
    public async Task<ActionResult<CharacterMountOuterDto>> GetMountsAsync(string lodestoneId, int? maxAge)
    {
        try
        {
            return await _characterService.GetCharacterMounts(lodestoneId, maxAge);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}