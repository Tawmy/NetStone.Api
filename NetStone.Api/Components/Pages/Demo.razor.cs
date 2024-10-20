using System.Text.Json;
using Microsoft.AspNetCore.Components;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;

namespace NetStone.Api.Components.Pages;

public partial class Demo : ComponentBase
{
    private const string CharacterLodestoneId = "28812634";
    private const string FreeCompanyLodestoneId = "9231253336202818312";

    private static readonly JsonSerializerOptions SerOptions = new() { WriteIndented = true };
    private string? _placeholder = "Press a button!";
    private string _result = string.Empty;

    #region Character Clicks

    /// <summary>
    ///     Character service for data retrieval
    /// </summary>
    [Inject]
    public ICharacterService CharacterService { get; set; } = default!;

    /// <summary>
    ///     Free Company service for data retrieval
    /// </summary>
    [Inject]
    public IFreeCompanyService FreeCompanyService { get; set; } = default!;

    private async Task CharacterSearchAsync()
    {
        _placeholder = "Searching for \"Alyx\" on Phoenix...";
        var query = new CharacterSearchQuery("Alyx", "Phoenix");
        var result = await CharacterService.SearchCharacterAsync(query, 1);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterProfileAsync()
    {
        _placeholder = $"Retrieving profile for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterAsync(CharacterLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterClassJobsAsync()
    {
        _placeholder = $"Retrieving jobs for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterClassJobsAsync(CharacterLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterMinionsAsync()
    {
        _placeholder = $"Retrieving minions for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterMinionsAsync(CharacterLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterMountsAsync()
    {
        _placeholder = $"Retrieving mounts for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterMountsAsync(CharacterLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterAchievementsAsync()
    {
        _placeholder = $"Retrieving achievements for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterAchievementsAsync(CharacterLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    #endregion

    #region Free Company Clicks

    private async Task FreeCompanySearchAsync()
    {
        _placeholder = "Searching for \"Dust\" on Phoenix...";
        var query = new FreeCompanySearchQuery("Dust", "Phoenix");
        var result = await FreeCompanyService.SearchFreeCompanyAsync(query, 1);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task FreeCompanyProfileAsync()
    {
        _placeholder = $"Retrieving profile for Dust Bunnies (DUST) on Phoenix (ID {CharacterLodestoneId})...";
        var result = await FreeCompanyService.GetFreeCompanyAsync(FreeCompanyLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task FreeCompanyMembersAsync()
    {
        _placeholder = $"Retrieving members for Dust Bunnies (DUST) on Phoenix (ID {CharacterLodestoneId})...";
        var result = await FreeCompanyService.GetFreeCompanyMembersAsync(FreeCompanyLodestoneId, null);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    #endregion
}