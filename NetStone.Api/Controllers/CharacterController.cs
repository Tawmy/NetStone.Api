using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Data.LegacyDtos;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Controllers.V1
{
    /// <summary>
    ///     Character Controller. Parses Lodestone for character data and directly returns it.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiVersion(1)]
    [Obsolete("This API version is deprecated. Please use version 2 instead, which includes caching.")]
    public class CharacterController(ILegacyCharacterService characterService) : ControllerBase
    {
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
        /// <returns>DTO containing the parsed character and some goodie properties.</returns>
        [HttpGet("{lodestoneId}")]
        public async Task<ActionResult<LegacyLodestoneCharacterDto>> GetAsync(string lodestoneId)
        {
            try
            {
                return await characterService.GetCharacterAsync(lodestoneId);
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
                return await characterService.GetCharacterClassJobsAsync(lodestoneId);
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
        /// <returns>Character minions.</returns>
        [HttpGet("Minions/{lodestoneId}")]
        public async Task<ActionResult<CharacterCollectable>> GetMinionsAsync(string lodestoneId)
        {
            try
            {
                return await characterService.GetCharacterMinions(lodestoneId);
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
                return await characterService.GetCharacterMounts(lodestoneId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}

namespace NetStone.Api.Controllers.V2
{
    /// <summary>
    ///     Character controller. Parses Lodestone for Character data and caches it, then returns it as DTOs.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiVersion(2)]
    public class CharacterController(ICharacterService characterService) : ControllerBase
    {
        /// <summary>
        ///     Search for character with provided search query.
        /// </summary>
        /// <param name="query">Search query, CharacterName and World are needed.</param>
        /// <param name="page">Which page of the paginated results to return.</param>
        /// <returns>Results returned from Lodestone.</returns>
        [HttpPost("Search")]
        public async Task<ActionResult<CharacterSearchPageDto>> SearchAsync(Common.Queries.CharacterSearchQuery query,
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
        /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
        /// <returns>DTO containing the parsed character and some goodie properties.</returns>
        [HttpGet("{lodestoneId}")]
        public async Task<ActionResult<CharacterDto>> GetAsync(string lodestoneId, int? maxAge,
            bool useFallback = false)
        {
            try
            {
                return await characterService.GetCharacterAsync(lodestoneId, maxAge, useFallback);
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
        /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
        /// <remarks>
        ///     If character was never cached using <see cref="GetAsync" />, <see cref="CharacterClassJobOuterDto.LastUpdated" />
        ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
        ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
        ///     <paramref name="maxAge" /> applies as expected.
        /// </remarks>
        /// <returns>Character class jobs.</returns>
        [HttpGet("ClassJobs/{lodestoneId}")]
        public async Task<ActionResult<CharacterClassJobOuterDto>> GetClassJobsAsync(string lodestoneId, int? maxAge,
            bool useFallback = false)
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
        /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
        /// <remarks>
        ///     If character was never cached using <see cref="GetAsync" />, <see cref="CollectionDto{t}.LastUpdated" />
        ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
        ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
        ///     <paramref name="maxAge" /> applies as expected.
        /// </remarks>
        /// <returns>Character minions.</returns>
        [HttpGet("Minions/{lodestoneId}")]
        public async Task<ActionResult<CollectionDto<CharacterMinionDto>>> GetMinionsAsync(string lodestoneId,
            int? maxAge, bool useFallback = false)
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
        /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
        /// <remarks>
        ///     If character was never cached using <see cref="GetAsync" />, <see cref="CollectionDto{T}.LastUpdated" />
        ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
        ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
        ///     <paramref name="maxAge" /> applies as expected.
        /// </remarks>
        /// <returns>Character mounts.</returns>
        [HttpGet("Mounts/{lodestoneId}")]
        public async Task<ActionResult<CollectionDto<CharacterMountDto>>> GetMountsAsync(string lodestoneId,
            int? maxAge, bool useFallback = false)
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
        /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
        /// <remarks>
        ///     If character was never cached using <see cref="GetAsync" />,
        ///     <see cref="CharacterAchievementOuterDto.LastUpdated" />
        ///     cannot be set. Its value will be null as a result. In this case, if <paramref name="maxAge" /> is set to ANY value,
        ///     the data will be refreshed. If Character was cached at least once and the value can be saved,
        ///     <paramref name="maxAge" /> applies as expected.
        /// </remarks>
        /// <returns>Character achievements.</returns>
        [HttpGet("Achievements/{lodestoneId}")]
        public async Task<ActionResult<CharacterAchievementOuterDto>> GetAchievementsAsync(string lodestoneId,
            int? maxAge, bool useFallback = false)
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
    }
}