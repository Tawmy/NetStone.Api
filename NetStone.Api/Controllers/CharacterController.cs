using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;

namespace NetStone.Api.Controllers;

/// <summary>
///     Character controller. Parses Lodestone for Character data and caches it, then returns it as DTOs.
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
[ApiVersion(4)]
public class CharacterController(ICharacterServiceV4 characterService) : ControllerBase
{
    /// <summary>
    ///     Search for character with provided search query.
    /// </summary>
    /// <param name="query">Search query, CharacterName and World are needed.</param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Results returned from Lodestone.</returns>
    [HttpPost("Search")]
    public async Task<ActionResult<CharacterSearchPageDto>> SearchAsync(CharacterSearchQuery query,
        int page = 1)
    {
        try
        {
            return await characterService.SearchCharacterAsync(query, page);
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
    ///     Optional maximum age of cached character, in minutes. If older, it will be refreshed from the Lodestone.
    /// </param>
    /// <param name="cacheImages">
    ///     Whether to download and storage avatar and portrait separately.
    ///     Please be mindful of storage requirements.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    [HttpGet("{lodestoneId}")]
    [ProducesResponseType<CharacterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CharacterDto>> GetAsync(string lodestoneId, int? maxAge, bool cacheImages = false,
        FallbackTypeV4 useFallback = FallbackTypeV4.None)
    {
        try
        {
            return await characterService.GetCharacterAsync(lodestoneId, maxAge, cacheImages, useFallback);
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
    ///     Optional maximum age of cached class jobs, in minutes. If older, they will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CharacterClassJobOuterDto.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
    ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
    ///     <paramref name="maxAge" /> applies as expected.
    /// </remarks>
    /// <returns>Character class jobs.</returns>
    [HttpGet("{lodestoneId}/ClassJobs")]
    [ProducesResponseType<CharacterClassJobOuterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CharacterClassJobOuterDto>> GetClassJobsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV4 useFallback = FallbackTypeV4.None)
    {
        try
        {
            return await characterService.GetCharacterClassJobsAsync(lodestoneId, maxAge, useFallback);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get a character's minions.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached minions, in minutes. If older, they will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CollectionDto{T}.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
    ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
    ///     <paramref name="maxAge" /> applies as expected.
    /// </remarks>
    /// <returns>Character minions.</returns>
    [HttpGet("{lodestoneId}/Minions")]
    [ProducesResponseType<CollectionDto<CharacterMinionDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CollectionDto<CharacterMinionDto>>> GetMinionsAsync(string lodestoneId,
        int? maxAge, FallbackTypeV4 useFallback = FallbackTypeV4.None)
    {
        try
        {
            return await characterService.GetCharacterMinionsAsync(lodestoneId, maxAge, useFallback);
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
    ///     Optional maximum age of cached mounts, in minutes. If older, they will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />, <see cref="CollectionDto{T}.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
    ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
    ///     <paramref name="maxAge" /> applies as expected.
    /// </remarks>
    /// <returns>Character mounts.</returns>
    [HttpGet("{lodestoneId}/Mounts")]
    [ProducesResponseType<CollectionDto<CharacterMountDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CollectionDto<CharacterMountDto>>> GetMountsAsync(string lodestoneId,
        int? maxAge, FallbackTypeV4 useFallback = FallbackTypeV4.None)
    {
        try
        {
            return await characterService.GetCharacterMountsAsync(lodestoneId, maxAge, useFallback);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get a character's achievements.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached achievements, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <remarks>
    ///     If character was never cached using <see cref="GetAsync" />,
    ///     <see cref="CharacterAchievementOuterDto.LastUpdated" />
    ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
    ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
    ///     <paramref name="maxAge" /> applies as expected.
    /// </remarks>
    /// <returns>Character achievements.</returns>
    [HttpGet("{lodestoneId}/Achievements")]
    [ProducesResponseType<CharacterAchievementOuterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CharacterAchievementOuterDto>> GetAchievementsAsync(string lodestoneId,
        int? maxAge, FallbackTypeV4 useFallback = FallbackTypeV4.None)
    {
        try
        {
            return await characterService.GetCharacterAchievementsAsync(lodestoneId, maxAge, useFallback);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get character with the given name and home world <b>FROM CACHE</b>.
    /// </summary>
    /// ///
    /// <param name="world">Home World, case insensitive.</param>
    /// <param name="name">Character name. Must be exact match, but is case insensitive.</param>
    /// <returns>DTO containing the character and some goodie properties.</returns>
    [HttpGet("{world}/{name}")]
    [ProducesResponseType<CharacterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CharacterDto>> GetByNameAsync(string world, string name)
    {
        try
        {
            return await characterService.GetCharacterByNameAsync(name, world);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}