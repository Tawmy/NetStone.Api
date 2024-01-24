using AutoMapper;
using NetStone.Api.DTOs;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Services;

public class CharacterService : ICharacterService
{
    private readonly LodestoneClient _client;
    private readonly IMapper _mapper;

    public CharacterService(LodestoneClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public Task<CharacterSearchPage?> SearchCharacterAsync(CharacterSearchQuery query)
    {
        return _client.SearchCharacter(query);
    }

    public async Task<LodestoneCharacterDto?> GetCharacterAsync(string lodestoneId)
    {
        return _mapper.Map<LodestoneCharacterDto>(await _client.GetCharacter(lodestoneId));
    }
}