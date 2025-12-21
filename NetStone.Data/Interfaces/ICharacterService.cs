using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;

namespace NetStone.Data.Interfaces;

/// <summary>
///     Data service for character data.
/// </summary>
public interface ICharacterServiceV4
{
    /// <summary>
    ///     Search for character with provided search query.
    /// </summary>
    /// <param name="query">
    ///     Search query, only <see cref="CharacterSearchQuery.CharacterName" /> and
    ///     <see cref="CharacterSearchQuery.World" /> are needed.
    /// </param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Instance of <see cref="CharacterSearchPageDto" /> with the results returned from the Lodestone.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached character, in minutes. If older, it will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if last updated field not set, indicates bug.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge, FallbackTypeV4 useFallback);

    /// <summary>
    ///     Get character with the given name and home world from cache.
    /// </summary>
    /// <param name="name">Character name. Must be exact match, but is case insensitive.</param>
    /// <param name="world">Home World, case insensitive.</param>
    /// <returns>DTO containing the character and some goodie properties.</returns>
    /// <exception cref="NotFoundException">Thrown if character not cached.</exception>
    /// <exception cref="InvalidOperationException">Thrown if last updated field not set, indicates bug.</exception>
    public Task<CharacterDto> GetCharacterByNameAsync(string name, string world);

    /// <summary>
    ///     Get a character's ClassJobs.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached class jobs, in minutes. If older, they will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>Character class jobs.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV4 useFallback);

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
    /// <returns>Character minions.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CollectionDto<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV4 useFallback);

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
    /// <returns>Character mounts.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CollectionDto<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV4 useFallback);

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
    /// <returns>Character achievements.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV4 useFallback);
}

/// <summary>
///     Data service for character data.
/// </summary>
public interface ICharacterServiceV3
{
    /// <summary>
    ///     Search for character with provided search query.
    /// </summary>
    /// <param name="query">
    ///     Search query, only <see cref="CharacterSearchQuery.CharacterName" /> and
    ///     <see cref="CharacterSearchQuery.World" /> are needed.
    /// </param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Instance of <see cref="CharacterSearchPageDto" /> with the results returned from the Lodestone.</returns>
    /// <exception cref="NotFoundException"></exception>
    public Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached character, in minutes. If older, it will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>DTO containing the parsed character and some goodie properties.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if last updated field not set, indicates bug.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge, FallbackTypeV3 useFallback);

    /// <summary>
    ///     Get character with the given name and home world from cache.
    /// </summary>
    /// <param name="name">Character name. Must be exact match, but is case insensitive.</param>
    /// <param name="world">Home World, case insensitive.</param>
    /// <returns>DTO containing the character and some goodie properties.</returns>
    /// <exception cref="NotFoundException">Thrown if character not cached.</exception>
    /// <exception cref="InvalidOperationException">Thrown if last updated field not set, indicates bug.</exception>
    public Task<CharacterDto> GetCharacterByNameAsync(string name, string world);

    /// <summary>
    ///     Get a character's ClassJobs.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use Use <see cref="SearchCharacterAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached class jobs, in minutes. If older, they will be refreshed from the Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>Character class jobs.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback);

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
    /// <returns>Character minions.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CollectionDto<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback);

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
    /// <returns>Character mounts.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    public Task<CollectionDto<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback);

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
    /// <returns>Character achievements.</returns>
    /// <exception cref="NotFoundException">Thrown if character not found.</exception>
    /// <exception cref="HttpRequestException">Thrown if fallback type is None and request failed.</exception>
    /// <exception cref="ParsingFailedException">Thrown if parsing failed (profile private or Lodestone under maintenance)</exception>
    Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback);
}