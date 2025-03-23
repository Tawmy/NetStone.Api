using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Collectable;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Search.Character;
using NetStone.Search.FreeCompany;

namespace NetStone.Cache.Interfaces;

public interface INetStoneService
{
    Task<CharacterSearchPage?> SearchCharacter(CharacterSearchQuery query, int page = 1);
    Task<LodestoneCharacter?> GetCharacter(string id);
    Task<CharacterClassJob?> GetCharacterClassJob(string id);
    Task<CharacterCollectable?> GetCharacterMinion(string id);
    Task<CharacterCollectable?> GetCharacterMount(string id);
    Task<CharacterAchievementPage?> GetCharacterAchievement(string id, int page = 1);

    Task<FreeCompanySearchPage?> SearchFreeCompany(FreeCompanySearchQuery query, int page = 1);
    Task<LodestoneFreeCompany?> GetFreeCompany(string id);
    Task<FreeCompanyMembers?> GetFreeCompanyMembers(string id, int page = 1);
}