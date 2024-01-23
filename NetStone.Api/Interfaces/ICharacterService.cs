using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Interfaces;

public interface ICharacterService
{
    public Task<CharacterSearchPage?> SearchCharacterAsync(CharacterSearchQuery query);

    public Task<LodestoneCharacter?> GetCharacterAsync(string lodestoneId);
}