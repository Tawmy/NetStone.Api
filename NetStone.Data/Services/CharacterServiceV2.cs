using System.Diagnostics;
using System.Text.Json;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Data.Services;

public class CharacterServiceV2(
    INetStoneService netStoneService,
    ICharacterCachingServiceV2 cachingService,
    ICharacterEventService eventService,
    CollectionDataService collectionData,
    IAutoMapperService mapper)
    : ICharacterServiceV2
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterServiceV2));

    public async Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(CharacterSearchQuery), JsonSerializer.Serialize(query));

        var netStoneQuery = mapper.Map<Search.Character.CharacterSearchQuery>(query);
        var result = await netStoneService.SearchCharacter(netStoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return mapper.Map<CharacterSearchPageDto>(result);
    }

    public async Task<CharacterDtoV2> GetCharacterAsync(string lodestoneId, int? maxAge)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedCharacterDto = await cachingService.GetCharacterAsync(lodestoneId);

        if (cachedCharacterDto is not null)
        {
            if (cachedCharacterDto.LastUpdated is null)
            {
                throw new InvalidOperationException($"{nameof(CharacterDtoV2.LastUpdated)} must never be null here.");
            }

            if ((DateTime.UtcNow - cachedCharacterDto.LastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
            {
                // return cached character if possible
                cachedCharacterDto = cachedCharacterDto with { Cached = true };
                return mapper.Map<CharacterDtoV2>(cachedCharacterDto);
            }
        }

        var lodestoneCharacter = await netStoneService.GetCharacter(lodestoneId);
        if (lodestoneCharacter is null) throw new NotFoundException();

        // cache character, send to queue, and return
        cachedCharacterDto = await cachingService.CacheCharacterAsync(lodestoneId, lodestoneCharacter);
        _ = eventService.CharacterRefreshedAsync(cachedCharacterDto);
        return mapper.Map<CharacterDtoV2>(cachedCharacterDto);
    }

    public async Task<CharacterClassJobOuterDtoV2> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge)
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
                    return new CharacterClassJobOuterDtoV2(cachedClassJobsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterClassJobOuterDtoV2(cachedClassJobsDtos, true, null);
            }
        }

        var lodestoneCharacterClassJobs = await netStoneService.GetCharacterClassJob(lodestoneId);
        if (lodestoneCharacterClassJobs is null) throw new NotFoundException();

        cachedClassJobsDtos =
            await cachingService.CacheCharacterClassJobsAsync(lodestoneId, lodestoneCharacterClassJobs);
        var outerDto = new CharacterClassJobOuterDtoV2(cachedClassJobsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterClassJobsRefreshedAsync(outerDto);
        return outerDto;
    }

    public async Task<CollectionDtoV2<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge)
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
                    return new CollectionDtoV2<CharacterMinionDto>(cachedMinionsDtos, true, lastUpdated.Value,
                        await collectionData.GetTotalMinionsAsync());
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDtoV2<CharacterMinionDto>(cachedMinionsDtos, true, null,
                    await collectionData.GetTotalMinionsAsync());
            }
        }

        var lodestoneMinions = await netStoneService.GetCharacterMinion(lodestoneId);
        if (lodestoneMinions is null) throw new NotFoundException();

        cachedMinionsDtos = await cachingService.CacheCharacterMinionsAsync(lodestoneId, lodestoneMinions);

        var collectionDto = new CollectionDtoV2<CharacterMinionDto>(cachedMinionsDtos, false, DateTime.UtcNow,
            await collectionData.GetTotalMinionsAsync());
        _ = eventService.CharacterMinionsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CollectionDtoV2<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge)
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
                    return new CollectionDtoV2<CharacterMountDto>(cachedMountsDtos, true, lastUpdated.Value,
                        await collectionData.GetTotalMountsAsync());
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDtoV2<CharacterMountDto>(cachedMountsDtos, true, null,
                    await collectionData.GetTotalMountsAsync());
            }
        }

        var lodestoneMounts = await netStoneService.GetCharacterMount(lodestoneId);
        if (lodestoneMounts is null) throw new NotFoundException();

        cachedMountsDtos = await cachingService.CacheCharacterMountsAsync(lodestoneId, lodestoneMounts);

        var collectionDto =
            new CollectionDtoV2<CharacterMountDto>(cachedMountsDtos, false, DateTime.UtcNow,
                await collectionData.GetTotalMountsAsync());
        _ = eventService.CharacterMountsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CharacterAchievementOuterDtoV2> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge)
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
                    return new CharacterAchievementOuterDtoV2(cachedAchievementsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterAchievementOuterDtoV2(cachedAchievementsDtos, true, null);
            }
        }

        var lodestoneAchievements = await RetrieveAllAchievementsAsync(lodestoneId);

        cachedAchievementsDtos =
            await cachingService.CacheCharacterAchievementsAsync(lodestoneId, lodestoneAchievements);

        var outerDto = new CharacterAchievementOuterDtoV2(cachedAchievementsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterAchievementsRefreshedAsync(outerDto);
        return outerDto;
    }

    private async Task<List<CharacterAchievementEntry>> RetrieveAllAchievementsAsync(string lodestoneId)
    {
        var page1 = await netStoneService.GetCharacterAchievement(lodestoneId);
        if (page1 is null) throw new NotFoundException();
        var achievements = page1.Achievements.ToList();

        var tasks = new List<Task<CharacterAchievementPage?>>();
        for (var i = page1.CurrentPage + 1; i <= page1.NumPages; i++)
        {
            tasks.Add(netStoneService.GetCharacterAchievement(lodestoneId, i));
        }

        await foreach (var task in Task.WhenEach(tasks))
        {
            if (task.Result is null) throw new NotFoundException();
            achievements.AddRange(task.Result.Achievements);
        }

        return achievements;
    }
}