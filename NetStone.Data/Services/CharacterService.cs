using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NetStone.Cache.Extensions;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Data.Services;

public class CharacterService(
    INetStoneService netStoneService,
    ICharacterCachingService cachingService,
    ICharacterEventService eventService,
    CollectionDataService collectionData,
    ILogger<CharacterService> logger)
    : ICharacterService
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterService));

    public async Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(CharacterSearchQuery), JsonSerializer.Serialize(query));

        var netStoneQuery = query.ToNetStone();
        var result = await netStoneService.SearchCharacter(netStoneQuery, page);
        if (result is not { HasResults: true })
        {
            throw new NotFoundException();
        }

        if (!result.Results.Any())
        {
            // parser returns HasResults true, but zero results during maintenance
            throw new ParsingFailedException(JsonSerializer.Serialize(query));
        }

        return result.ToDto();
    }

    public async Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge, FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedCharacterDto = await cachingService.GetCharacterAsync(lodestoneId);

        if (cachedCharacterDto is not null)
        {
            if (cachedCharacterDto.LastUpdated is null)
            {
                throw new InvalidOperationException($"{nameof(CharacterDto.LastUpdated)} must never be null here.");
            }

            if ((DateTime.UtcNow - cachedCharacterDto.LastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
            {
                // return cached character if possible
                return cachedCharacterDto with { Cached = true };
            }
        }

        LodestoneCharacter? lodestoneCharacter;
        try
        {
            lodestoneCharacter = await netStoneService.GetCharacter(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedCharacterDto is not null && (useFallback is FallbackType.Any ||
                                                   (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetCharacterAsync), ex.Message);
                return cachedCharacterDto with { Cached = true, FallbackUsed = true, FallbackReason = ex.Message };
            }

            throw;
        }

        if (lodestoneCharacter is null)
        {
            throw new NotFoundException();
        }

        if (!lodestoneCharacter.IsFullyPublic())
        {
            if (cachedCharacterDto is not null && useFallback is FallbackType.Any)
            {
                return cachedCharacterDto with
                {
                    Cached = true, FallbackUsed = true, FallbackReason = nameof(ParsingFailedException)
                };
            }

            throw new ParsingFailedException(lodestoneId);
        }

        // cache character, send to queue, and return
        cachedCharacterDto = await cachingService.CacheCharacterAsync(lodestoneId, lodestoneCharacter);
        _ = eventService.CharacterRefreshedAsync(cachedCharacterDto);
        return cachedCharacterDto;
    }

    public async Task<CharacterDto> GetCharacterByNameAsync(string name, string world)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedCharacterDto = await cachingService.GetCharacterAsync(name, world);

        if (cachedCharacterDto is null)
        {
            throw new NotFoundException();
        }

        if (cachedCharacterDto.LastUpdated is null)
        {
            throw new InvalidOperationException($"{nameof(CharacterDto.LastUpdated)} must never be null here.");
        }

        return cachedCharacterDto with { Cached = true };
    }

    public async Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge,
        FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var (cachedClassJobsDtos, lastUpdated) = await cachingService.GetCharacterClassJobsAsync(lodestoneId);

        if (cachedClassJobsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time ClassJobs were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CharacterClassJobOuterDto(cachedClassJobsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterClassJobOuterDto(cachedClassJobsDtos, true, null);
            }
        }

        CharacterClassJob? lodestoneCharacterClassJobs;
        try
        {
            lodestoneCharacterClassJobs = await netStoneService.GetCharacterClassJob(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedClassJobsDtos.Any() && (useFallback is FallbackType.Any ||
                                              (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetCharacterClassJobsAsync), ex.Message);
                return new CharacterClassJobOuterDto(cachedClassJobsDtos, true, lastUpdated, true, ex.Message);
            }

            throw;
        }

        if (lodestoneCharacterClassJobs is null)
        {
            throw new NotFoundException();
        }

        if (string.IsNullOrWhiteSpace(lodestoneCharacterClassJobs.Alchemist.Name))
        {
            if (cachedClassJobsDtos.Any() && useFallback is FallbackType.Any)
            {
                return new CharacterClassJobOuterDto(cachedClassJobsDtos, true, lastUpdated, true,
                    nameof(ParsingFailedException));
            }

            throw new ParsingFailedException(lodestoneId);
        }

        cachedClassJobsDtos = await cachingService.CacheCharacterClassJobsAsync(lodestoneId,
            lodestoneCharacterClassJobs);

        var outerDto = new CharacterClassJobOuterDto(cachedClassJobsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterClassJobsRefreshedAsync(outerDto);
        return outerDto;
    }

    public async Task<CollectionDto<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge,
        FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var (cachedMinionsDtos, lastUpdated) = await cachingService.GetCharacterMinionsAsync(lodestoneId);

        if (cachedMinionsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time minions were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, lastUpdated.Value,
                        await collectionData.GetTotalMinionsAsync());
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, null,
                    await collectionData.GetTotalMinionsAsync());
            }
        }

        CharacterCollectable? lodestoneMinions;
        try
        {
            lodestoneMinions = await netStoneService.GetCharacterMinion(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedMinionsDtos.Any() && (useFallback is FallbackType.Any ||
                                            (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetCharacterMinionsAsync), ex.Message);
                return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, lastUpdated,
                    await collectionData.GetTotalMinionsAsync(), true, ex.Message);
            }

            throw;
        }

        if (lodestoneMinions is null)
        {
            throw new NotFoundException();
        }

        if (!lodestoneMinions.Collectables.Any() && cachedMinionsDtos.Any())
        {
            // no minions returned, but minions were cached before -> Lodestone under maintenance or profile private
            // we cannot always throw when no minions are returned, as new character might actually have none
            if (useFallback is FallbackType.Any)
            {
                return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, lastUpdated,
                    await collectionData.GetTotalMinionsAsync(), true, nameof(ParsingFailedException));
            }

            throw new ParsingFailedException(lodestoneId);
        }

        cachedMinionsDtos = await cachingService.CacheCharacterMinionsAsync(lodestoneId, lodestoneMinions);

        var collectionDto = new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, false, DateTime.UtcNow,
            await collectionData.GetTotalMinionsAsync());
        _ = eventService.CharacterMinionsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CollectionDto<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge,
        FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var (cachedMountsDtos, lastUpdated) = await cachingService.GetCharacterMountsAsync(lodestoneId);

        if (cachedMountsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time mounts were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, lastUpdated.Value,
                        await collectionData.GetTotalMountsAsync());
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, null,
                    await collectionData.GetTotalMountsAsync());
            }
        }

        CharacterCollectable? lodestoneMounts;
        try
        {
            lodestoneMounts = await netStoneService.GetCharacterMount(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedMountsDtos.Any() && (useFallback is FallbackType.Any ||
                                           (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetCharacterMountsAsync), ex.Message);
                return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, lastUpdated,
                    await collectionData.GetTotalMountsAsync(), true, ex.Message);
            }

            throw;
        }

        if (lodestoneMounts is null)
        {
            throw new NotFoundException();
        }

        if (!lodestoneMounts.Collectables.Any() && cachedMountsDtos.Any())
        {
            // no minions returned, but minions were cached before -> Lodestone under maintenance or profile private
            // we cannot always throw when no mounts are returned, as new character might actually have none
            if (useFallback is FallbackType.Any)
            {
                return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, lastUpdated,
                    await collectionData.GetTotalMountsAsync(), true, nameof(ParsingFailedException));
            }

            throw new ParsingFailedException(lodestoneId);
        }

        cachedMountsDtos = await cachingService.CacheCharacterMountsAsync(lodestoneId, lodestoneMounts);

        var collectionDto = new CollectionDto<CharacterMountDto>(cachedMountsDtos, false, DateTime.UtcNow,
            await collectionData.GetTotalMountsAsync());
        _ = eventService.CharacterMountsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge,
        FallbackType useFallback)
    {
        using var activity = ActivitySource.StartActivity();

        var (cachedAchievementsDtos, lastUpdated) = await cachingService.GetCharacterAchievementsAsync(lodestoneId);

        if (cachedAchievementsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time achievements were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CharacterAchievementOuterDto(cachedAchievementsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterAchievementOuterDto(cachedAchievementsDtos, true, null);
            }
        }

        IList<CharacterAchievementEntry>? lodestoneAchievements;
        try
        {
            lodestoneAchievements = await RetrieveAllAchievementsAsync(lodestoneId);
        }
        catch (Exception ex)
        {
            if (cachedAchievementsDtos.Any() && (useFallback is FallbackType.Any ||
                                                 (useFallback is FallbackType.Http && ex is HttpRequestException)))
            {
                logger.LogWarning("Fallback used for ID {id} in {method}: {msg}", lodestoneId,
                    nameof(GetCharacterAchievementsAsync), ex.Message);
                return new CharacterAchievementOuterDto(cachedAchievementsDtos, true, lastUpdated, true, ex.Message);
            }

            if (ex is FormatException)
            {
                if (cachedAchievementsDtos.Any() && useFallback is FallbackType.Any)
                {
                    return new CharacterAchievementOuterDto(cachedAchievementsDtos, true, lastUpdated, true,
                        ex.Message);
                }

                throw new ParsingFailedException(lodestoneId);
            }

            throw;
        }

        cachedAchievementsDtos =
            await cachingService.CacheCharacterAchievementsAsync(lodestoneId, lodestoneAchievements);

        var outerDto = new CharacterAchievementOuterDto(cachedAchievementsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterAchievementsRefreshedAsync(outerDto);
        return outerDto;
    }

    private async Task<List<CharacterAchievementEntry>> RetrieveAllAchievementsAsync(string lodestoneId)
    {
        // do not retrieve pages in parallel, it tends to run into rate limit

        if (await netStoneService.GetCharacterAchievement(lodestoneId) is not { } page1)
        {
            throw new NotFoundException();
        }

        var achievements = page1.Achievements.ToList();

        for (var i = page1.CurrentPage + 1; i <= page1.NumPages; i++)
        {
            var page = await netStoneService.GetCharacterAchievement(lodestoneId, i);
            if (page == null) throw new NotFoundException();
            achievements.AddRange(page.Achievements);
        }

        return achievements;
    }
}