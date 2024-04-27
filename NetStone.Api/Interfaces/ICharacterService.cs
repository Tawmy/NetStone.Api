using NetStone.Api.Controllers;
using NetStone.Common.DTOs;
using NetStone.Common.Exceptions;
using NetStone.Model.Parseables.Character.Achievement;
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
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Instance of <see cref="CharacterSearchPage" /> with the results returned from the Lodestone.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterSearchPage> SearchCharacterAsync(CharacterSearchQuery query, int page);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached character, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge);

    /// <summary>
    ///     Get a character's ClassJobs.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached class jobs, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>Character class jobs.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge);

    /// <summary>
    ///     Get a character's achievements.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Character achievements.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterAchievementPage> GetCharacterAchievements(string lodestoneId, int page);

    /// <summary>
    ///     Get a character's minions.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached minions, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>Character minions.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterMinionOuterDto> GetCharacterMinions(string lodestoneId, int? maxAge);

    /// <summary>
    ///     Get a character's mounts.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached mounts, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>Character mounts.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterMountOuterDto> GetCharacterMounts(string lodestoneId, int? maxAge);
}