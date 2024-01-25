using AutoMapper;
using NetStone.Api.DTOs;
using NetStone.Api.Exceptions;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Services;

internal class CharacterService : ICharacterService
{
    private readonly LodestoneClient _client;
    private readonly IMapper _mapper;

    public CharacterService(LodestoneClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<CharacterSearchPage> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        var result = await _client.SearchCharacter(query, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result;
    }

    public async Task<LodestoneCharacterDto> GetCharacterAsync(string lodestoneId)
    {
        var character = await _client.GetCharacter(lodestoneId);
        if (character == null) throw new NotFoundException();
        return _mapper.Map<LodestoneCharacterDto>(character);
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