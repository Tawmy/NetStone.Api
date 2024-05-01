using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Data.Services;

internal class CharacterService : ICharacterService
{
    private readonly ICharacterCachingService _cachingService;
    private readonly LodestoneClient _client;

    public CharacterService(LodestoneClient client, ICharacterCachingService cachingService)
    {
        _client = client;
        _cachingService = cachingService;
    }

    public async Task<CharacterSearchPage> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        var result = await _client.SearchCharacter(query, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result;
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

    public async Task<CharacterAchievementPage> GetCharacterAchievementsAsync(string lodestoneId, int page)
    {
        var client = await LodestoneClient.GetClientAsync();
        var result = await client.GetCharacterAchievement(lodestoneId, page);
        if (result == null) throw new NotFoundException();

        return result;
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
}