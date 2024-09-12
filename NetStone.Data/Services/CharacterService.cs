using AutoMapper;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Data.Services;

internal class CharacterService : ICharacterService
{
    private readonly ICharacterCachingService _cachingService;
    private readonly LodestoneClient _client;
    private readonly IMapper _mapper;

    public CharacterService(LodestoneClient client, ICharacterCachingService cachingService, IMapper mapper)
    {
        _client = client;
        _cachingService = cachingService;
        _mapper = mapper;
    }

    public async Task<CharacterSearchPageDto> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        var netStoneQuery = _mapper.Map<Search.Character.CharacterSearchQuery>(query);
        var result = await _client.SearchCharacter(netStoneQuery, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return _mapper.Map<CharacterSearchPageDto>(result);
    }

    public async Task<CharacterDto> GetCharacterAsync(string lodestoneId, int? maxAge)
    {
        var cachedCharacterDto = await _cachingService.GetCharacterAsync(lodestoneId);

        if (cachedCharacterDto is not null &&
            (DateTime.UtcNow - cachedCharacterDto.LastUpdated).TotalMinutes <= (maxAge ?? int.MaxValue))
        {
            // return cached character if possible
            return cachedCharacterDto with { Cached = true };
        }

        var lodestoneCharacter = await _client.GetCharacter(lodestoneId);
        if (lodestoneCharacter == null) throw new NotFoundException();

        // cache character and return
        return await _cachingService.CacheCharacterAsync(lodestoneId, lodestoneCharacter);
    }

    public async Task<CharacterClassJobOuterDto> GetCharacterClassJobsAsync(string lodestoneId, int? maxAge)
    {
        var (cachedClassJobsDtos, lastUpdated) = await _cachingService.GetCharacterClassJobsAsync(lodestoneId);

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

        var lodestoneCharacterClassJobs = await _client.GetCharacterClassJob(lodestoneId);
        if (lodestoneCharacterClassJobs == null) throw new NotFoundException();

        cachedClassJobsDtos =
            await _cachingService.CacheCharacterClassJobsAsync(lodestoneId, lodestoneCharacterClassJobs);
        return new CharacterClassJobOuterDto(cachedClassJobsDtos, false, DateTime.UtcNow);
    }

    public async Task<CharacterMinionOuterDto> GetCharacterMinionsAsync(string lodestoneId, int? maxAge)
    {
        var (cachedMinionsDtos, lastUpdated) = await _cachingService.GetCharacterMinionsAsync(lodestoneId);

        if (cachedMinionsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time minions were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CharacterMinionOuterDto(cachedMinionsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterMinionOuterDto(cachedMinionsDtos, true, null);
            }
        }

        var lodestoneMinions = await _client.GetCharacterMinion(lodestoneId);
        if (lodestoneMinions == null) throw new NotFoundException();

        cachedMinionsDtos = await _cachingService.CacheCharacterMinionsAsync(lodestoneId, lodestoneMinions);

        return new CharacterMinionOuterDto(cachedMinionsDtos, false, DateTime.UtcNow);
    }

    public async Task<CharacterMountOuterDto> GetCharacterMountsAsync(string lodestoneId, int? maxAge)
    {
        var (cachedMountsDtos, lastUpdated) = await _cachingService.GetCharacterMountsAsync(lodestoneId);

        if (cachedMountsDtos.Any())
        {
            if (lastUpdated is not null)
            {
                if ((DateTime.UtcNow - lastUpdated.Value).TotalMinutes <= (maxAge ?? int.MaxValue))
                {
                    // if character was cached before, last time mounts were cached can be saved.
                    // If cache is not older than the max age submitted, return cache.
                    return new CharacterMountOuterDto(cachedMountsDtos, true, lastUpdated.Value);
                }
            }
            else if (maxAge is null)
            {
                // Character was never cached, so LastUpdated value cannot be saved.
                // If no max age given, return. If any max age value given, refresh.
                return new CharacterMountOuterDto(cachedMountsDtos, true, null);
            }
        }

        var lodestoneMounts = await _client.GetCharacterMount(lodestoneId);
        if (lodestoneMounts == null) throw new NotFoundException();

        cachedMountsDtos = await _cachingService.CacheCharacterMountsAsync(lodestoneId, lodestoneMounts);

        return new CharacterMountOuterDto(cachedMountsDtos, false, DateTime.UtcNow);
    }

    public async Task<CharacterAchievementOuterDto> GetCharacterAchievementsAsync(string lodestoneId, int? maxAge)
    {
        var (cachedAchievementsDtos, lastUpdated) = await _cachingService.GetCharacterAchievementsAsync(lodestoneId);

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
            await _cachingService.CacheCharacterAchievementsAsync(lodestoneId, lodestoneAchievements);

        return new CharacterAchievementOuterDto(cachedAchievementsDtos, false, DateTime.UtcNow);
    }

    private async Task<List<CharacterAchievementEntry>> RetrieveAllAchievementsAsync(string lodestoneId)
    {
        var page1 = await _client.GetCharacterAchievement(lodestoneId);
        if (page1 == null) throw new NotFoundException();
        var achievements = page1.Achievements.ToList();

        for (var i = page1.CurrentPage + 1; i <= page1.NumPages; i++)
        {
            var page = await _client.GetCharacterAchievement(lodestoneId, i);
            if (page == null) throw new NotFoundException();
            achievements.AddRange(page.Achievements);
        }

        return achievements;
    }
}