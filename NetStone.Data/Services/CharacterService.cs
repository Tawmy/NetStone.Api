using AutoMapper;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Data.Services;

internal class CharacterService(
    LodestoneClient client,
    ICharacterCachingService cachingService,
    ICharacterEventService eventService,
    IMapper mapper)
    : ICharacterService
{
    public async Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        var netStoneQuery = mapper.Map<Search.Character.CharacterSearchQuery>(query);
        var result = await client.SearchCharacter(netStoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return mapper.Map<CharacterSearchPageDto>(result);
    }

    public async Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge, bool useFallback)
    {
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
            lodestoneCharacter = await client.GetCharacter(lodestoneId);
        }
        catch (Exception ex)
        {
            if (useFallback && cachedCharacterDto is not null)
            {
                return cachedCharacterDto with { Cached = true, FallbackUsed = true, FallbackReason = ex.Message };
            }

            throw;
        }

        if (lodestoneCharacter is null)
        {
            throw new NotFoundException();
        }

        // cache character, send to queue, and return
        cachedCharacterDto = await cachingService.CacheCharacterAsync(lodestoneId, lodestoneCharacter);
        _ = eventService.CharacterRefreshedAsync(cachedCharacterDto);
        return cachedCharacterDto;
    }

    public async Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge,
        bool useFallback)
    {
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
            lodestoneCharacterClassJobs = await client.GetCharacterClassJob(lodestoneId);
        }
        catch (Exception ex)
        {
            if (useFallback && cachedClassJobsDtos.Any())
            {
                return new CharacterClassJobOuterDto(cachedClassJobsDtos, true, lastUpdated, true, ex.Message);
            }

            throw;
        }

        if (lodestoneCharacterClassJobs is null)
        {
            throw new NotFoundException();
        }

        cachedClassJobsDtos = await cachingService.CacheCharacterClassJobsAsync(lodestoneId,
            lodestoneCharacterClassJobs);

        var outerDto = new CharacterClassJobOuterDto(cachedClassJobsDtos, false, DateTime.UtcNow);
        _ = eventService.CharacterClassJobsRefreshedAsync(outerDto);
        return outerDto;
    }

    public async Task<CollectionDto<CharacterMinionDto>> GetCharacterMinionsAsync(string lodestoneId, int? maxAge,
        bool useFallback)
    {
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

        CharacterCollectable? lodestoneMinions;
        try
        {
            lodestoneMinions = await client.GetCharacterMinion(lodestoneId);
        }
        catch (Exception ex)
        {
            if (useFallback && cachedMinionsDtos.Any())
            {
                return new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, true, lastUpdated,
                    StaticValues.TotalMinions, true, ex.Message);
            }

            throw;
        }

        if (lodestoneMinions is null)
        {
            throw new NotFoundException();
        }

        cachedMinionsDtos = await cachingService.CacheCharacterMinionsAsync(lodestoneId, lodestoneMinions);

        var collectionDto = new CollectionDto<CharacterMinionDto>(cachedMinionsDtos, false, DateTime.UtcNow,
            StaticValues.TotalMinions);
        _ = eventService.CharacterMinionsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CollectionDto<CharacterMountDto>> GetCharacterMountsAsync(string lodestoneId, int? maxAge,
        bool useFallback)
    {
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

        CharacterCollectable? lodestoneMounts;
        try
        {
            lodestoneMounts = await client.GetCharacterMount(lodestoneId);
        }
        catch (Exception ex)
        {
            if (useFallback && cachedMountsDtos.Any())
            {
                return new CollectionDto<CharacterMountDto>(cachedMountsDtos, true, lastUpdated,
                    StaticValues.TotalMounts, true, ex.Message);
            }

            throw;
        }

        if (lodestoneMounts is null)
        {
            throw new NotFoundException();
        }

        cachedMountsDtos = await cachingService.CacheCharacterMountsAsync(lodestoneId, lodestoneMounts);

        var collectionDto =
            new CollectionDto<CharacterMountDto>(cachedMountsDtos, false, DateTime.UtcNow, StaticValues.TotalMounts);
        _ = eventService.CharacterMountsRefreshedAsync(collectionDto);
        return collectionDto;
    }

    public async Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge,
        bool useFallback)
    {
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
            if (useFallback && cachedAchievementsDtos.Any())
            {
                return new CharacterAchievementOuterDto(cachedAchievementsDtos, true, lastUpdated, true, ex.Message);
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
        if (await client.GetCharacterAchievement(lodestoneId) is not { } page1)
        {
            throw new NotFoundException();
        }

        var achievements = page1.Achievements.ToList();

        for (var i = page1.CurrentPage + 1; i <= page1.NumPages; i++)
        {
            var page = await client.GetCharacterAchievement(lodestoneId, i);
            if (page == null) throw new NotFoundException();
            achievements.AddRange(page.Achievements);
        }

        return achievements;
    }
}