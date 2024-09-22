using AutoMapper;
using NetStone.Common.Exceptions;
using NetStone.Data.Interfaces;
using NetStone.Data.LegacyDtos;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Data.Services;

public class LegacyCharacterService(LodestoneClient client, IMapper mapper) : ILegacyCharacterService
{
    public async Task<CharacterSearchPage> SearchCharacterAsync(CharacterSearchQuery query, int page)
    {
        var result = await client.SearchCharacter(query, page);
        if (result is not { HasResults: true }) throw new NotFoundException();

        return result;
    }

    public async Task<LegacyLodestoneCharacterDto> GetCharacterAsync(string lodestoneId)
    {
        var character = await client.GetCharacter(lodestoneId);
        if (character == null) throw new NotFoundException();
        return mapper.Map<LegacyLodestoneCharacterDto>(character);
    }

    public async Task<CharacterClassJob> GetCharacterClassJobsAsync(string lodestoneId)
    {
        var result = await client.GetCharacterClassJob(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<CharacterCollectable> GetCharacterMinions(string lodestoneId)
    {
        var result = await client.GetCharacterMinion(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }

    public async Task<CharacterCollectable> GetCharacterMounts(string lodestoneId)
    {
        var result = await client.GetCharacterMount(lodestoneId);
        if (result == null) throw new NotFoundException();

        return result;
    }
}