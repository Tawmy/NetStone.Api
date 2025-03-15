using System.Diagnostics;
using NetStone.Cache.Interfaces;
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

namespace NetStone.Cache.Services;

internal class NetStoneService(LodestoneClient lodestoneClient) : INetStoneService
{
    private static readonly ActivitySource ActivitySource = new(nameof(INetStoneService));

    public async Task<CharacterSearchPage?> SearchCharacter(CharacterSearchQuery query, int page = 1)
    {
        // do not return task directly as it would be executed outside the activity
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.SearchCharacter(query, page);
    }

    public async Task<LodestoneCharacter?> GetCharacter(string id)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetCharacter(id);
    }

    public async Task<CharacterClassJob?> GetCharacterClassJob(string id)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetCharacterClassJob(id);
    }

    public async Task<CharacterCollectable?> GetCharacterMinion(string id)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetCharacterMinion(id);
    }

    public async Task<CharacterCollectable?> GetCharacterMount(string id)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetCharacterMount(id);
    }

    public async Task<CharacterAchievementPage?> GetCharacterAchievement(string id, int page = 1)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetCharacterAchievement(id, page);
    }

    public async Task<FreeCompanySearchPage?> SearchFreeCompany(FreeCompanySearchQuery query, int page = 1)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.SearchFreeCompany(query, page);
    }

    public async Task<LodestoneFreeCompany?> GetFreeCompany(string id)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetFreeCompany(id);
    }

    public async Task<FreeCompanyMembers?> GetFreeCompanyMembers(string id, int page = 1)
    {
        using var activity = ActivitySource.StartActivity();
        return await lodestoneClient.GetFreeCompanyMembers(id, page);
    }
}