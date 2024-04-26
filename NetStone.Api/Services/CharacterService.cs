using AutoMapper;
using NetStone.Api.Exceptions;
using NetStone.Api.Interfaces;
using NetStone.Common.DTOs;
using NetStone.Common.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Services;

internal class CharacterService : ICharacterService
{
    private readonly ICharacterCachingService _cachingService;
    private readonly LodestoneClient _client;
    private readonly IMapper _mapper;

    public CharacterService(LodestoneClient client, IMapper mapper, ICharacterCachingService cachingService)
    {
        _client = client;
        _mapper = mapper;
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

        if (cachedCharacterDto != null &&
            (DateTime.UtcNow - cachedCharacterDto.LastUpdated).TotalMinutes <= (maxAge ?? int.MaxValue))
        {
            // return cached character if possible
            return cachedCharacterDto;
        }

        var lodestoneCharacter = await _client.GetCharacter(lodestoneId);
        if (lodestoneCharacter == null) throw new NotFoundException();

        // cache character and return
        return await _cachingService.CacheCharacterAsync(lodestoneCharacter, lodestoneId);
    }

    public async Task<CharacterClassJob> GetCharacterClassJobsAsync(string lodestoneId)
    {
        var result = await _client.GetCharacterClassJob(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<CharacterAchievementPage> GetCharacterAchievements(string lodestoneId, int page)
    {
        var client = await LodestoneClient.GetClientAsync();
        var result = await client.GetCharacterAchievement(lodestoneId, page);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<CharacterCollectable> GetCharacterMinions(string lodestoneId)
    {
        var result = await _client.GetCharacterMinion(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<CharacterCollectable> GetCharacterMounts(string lodestoneId)
    {
        var result = await _client.GetCharacterMount(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }
}