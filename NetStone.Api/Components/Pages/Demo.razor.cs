using System.Text.Json;
using Microsoft.AspNetCore.Components;
using NetStone.Common.Enums;
using NetStone.Data.Interfaces;

namespace NetStone.Api.Components.Pages;

/// <summary>
///     Logic for Demo page.
/// </summary>
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
    public ICharacterServiceV4 CharacterService { get; set; } = default!;

    /// <summary>
    ///     Free Company service for data retrieval
    /// </summary>
    [Inject]
    public IFreeCompanyServiceV4 FreeCompanyService { get; set; } = default!;

    private async Task CharacterProfileAsync()
    {
        _placeholder = $"Retrieving profile for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterAsync(CharacterLodestoneId, null, FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterClassJobsAsync()
    {
        _placeholder = $"Retrieving jobs for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterClassJobsAsync(CharacterLodestoneId, null, FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterMinionsAsync()
    {
        _placeholder = $"Retrieving minions for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterMinionsAsync(CharacterLodestoneId, null, FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterMountsAsync()
    {
        _placeholder = $"Retrieving mounts for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterMountsAsync(CharacterLodestoneId, null, FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task CharacterAchievementsAsync()
    {
        _placeholder = $"Retrieving achievements for Alyx Bergen on Phoenix (ID {CharacterLodestoneId})...";
        var result = await CharacterService.GetCharacterAchievementsAsync(CharacterLodestoneId, null,
            FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    #endregion

    #region Free Company Clicks

    private async Task FreeCompanyProfileAsync()
    {
        _placeholder = $"Retrieving profile for Dust Bunnies (DUST) on Phoenix (ID {CharacterLodestoneId})...";
        var result = await FreeCompanyService.GetFreeCompanyAsync(FreeCompanyLodestoneId, null, FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    private async Task FreeCompanyMembersAsync()
    {
        _placeholder = $"Retrieving members for Dust Bunnies (DUST) on Phoenix (ID {CharacterLodestoneId})...";
        var result = await FreeCompanyService.GetFreeCompanyMembersAsync(FreeCompanyLodestoneId, null,
            FallbackTypeV4.Any);
        _result = JsonSerializer.Serialize(result, SerOptions);
        _placeholder = null;
    }

    #endregion
}