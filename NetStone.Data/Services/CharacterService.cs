using System.Diagnostics;
using System.Text.Json;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Data.Services;

internal class CharacterService(
    INetStoneService netStoneService,
    ICharacterCachingService cachingService,
    ICharacterEventService eventService,
    IAutoMapperService mapper)
    : ICharacterService
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterService));

    public async Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.AddTag(nameof(CharacterSearchQuery), JsonSerializer.Serialize(query));

        var netStoneQuery = mapper.Map<Search.Character.CharacterSearchQuery>(query);
        var result = await netStoneService.SearchCharacter(netStoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return mapper.Map<CharacterSearchPageDto>(result);
    }

    public async Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge)
    {
        using var activity = ActivitySource.StartActivity();

        var cachedCharacterDto = await cachingService.GetCharacterAsync(lodestoneId);

        if (cachedCharacterDto is not null &&
            (DateTime.UtcNow - cachedCharacterDto.LastUpdated).TotalMinutes <= (maxAge ?? int.MaxValue))
        {
            // return cached character if possible
            return cachedCharacterDto with { Cached = true };
        }

        var lodestoneCharacter = await netStoneService.GetCharacter(lodestoneId);
        if (lodestoneCharacter is null) throw new NotFoundException();

        // cache character, send to queue, and return
        cachedCharacterDto = await cachingService.CacheCharacterAsync(lodestoneId, lodestoneCharacter);
        _ = eventService.CharacterRefreshedAsync(cachedCharacterDto);
        return cachedCharacterDto;
    }

    public async Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge)
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

        var lodestoneCharacterClassJobs = await netStoneService.GetCharacterClassJob(lodestoneId);
        if (lodestoneCharacterClassJobs is null) throw new NotFoundException();

        cachedClassJobsDtos =
            await cachingService.CacheCharacterClassJobsAsync(lodestoneId, lodestoneCharacterClassJobs);
        var outerDto = new CharacterClassJobOuterDto(cachedClassJobsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterClassJobsRefreshedAsync(outerDto);
        return outerDto;
    }

    public async Task<CollectionDto<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge)
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
                        StaticValues.TotalMinions);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, null, StaticValues.TotalMinions);
            }
        }

        var lodestoneMinions = await netStoneService.GetCharacterMinion(lodestoneId);
        if (lodestoneMinions is null) throw new NotFoundException();

        cachedMinionsDtos = await cachingService.CacheCharacterMinionsAsync(lodestoneId, lodestoneMinions);

        var collectionDto = new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, false, DateTime.UtcNow,
            StaticValues.TotalMinions);
        _ = eventService.CharacterMinionsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CollectionDto<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge)
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
                        StaticValues.TotalMounts);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, null, StaticValues.TotalMounts);
            }
        }

        var lodestoneMounts = await netStoneService.GetCharacterMount(lodestoneId);
        if (lodestoneMounts is null) throw new NotFoundException();

        cachedMountsDtos = await cachingService.CacheCharacterMountsAsync(lodestoneId, lodestoneMounts);

        var collectionDto =
            new CollectionDto<CharacterMountDto>(cachedMountsDtos, false, DateTime.UtcNow, StaticValues.TotalMounts);
        _ = eventService.CharacterMountsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge)
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

        var lodestoneAchievements = await RetrieveAllAchievementsAsync(lodestoneId);

        cachedAchievementsDtos =
            await cachingService.CacheCharacterAchievementsAsync(lodestoneId, lodestoneAchievements);

        var outerDto = new CharacterAchievementOuterDto(cachedAchievementsDtos, false, DateTime.UtcNow);
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