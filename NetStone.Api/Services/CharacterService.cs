using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Services;

public class CharacterService : ICharacterService
{
    private readonly LodestoneClient _client;

    public CharacterService(LodestoneClient client)
    {
        _client = client;
    }

    public Task<CharacterSearchPage?> SearchCharacterAsync(CharacterSearchQuery query)
    {
        return _client.SearchCharacter(query);
    }

    public Task<LodestoneCharacter?> GetCharacterAsync(string lodestoneId)
    {
        return _client.GetCharacter(lodestoneId);
    }
}