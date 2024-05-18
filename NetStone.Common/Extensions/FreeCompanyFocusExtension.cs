using NetStone.Common.Enums;

namespace NetStone.Common.Extensions;

public static class FreeCompanyFocusExtension
{
    public static string GetFocusIcon(this FreeCompanyFocus focus)
    {
        return focus switch
        {
            FreeCompanyFocus.None => string.Empty,
            FreeCompanyFocus.RolePlay => "https://lds-img.finalfantasyxiv.com/h/9/2RIcg3Swu7asLE9w5hF11Gm1Sg.png",
            FreeCompanyFocus.Leveling => "https://lds-img.finalfantasyxiv.com/h/n/5Y0D3iH7ngHlRpv9-KJKalt3_o.png",
            FreeCompanyFocus.Casual => "https://lds-img.finalfantasyxiv.com/h/t/Se_50UHNcTNjytzG8olOSGq3MI.png",
            FreeCompanyFocus.Hardcore => "https://lds-img.finalfantasyxiv.com/h/i/1ZVA5nBRhczPW2kJWBJk5Jkz6o.png",
            FreeCompanyFocus.Dungeons => "https://lds-img.finalfantasyxiv.com/h/7/jSYmuDYlpVzQ98RA3VhBQWY5PA.png",
            FreeCompanyFocus.Guildhests => "https://lds-img.finalfantasyxiv.com/h/C/wiboHfsKd8h_tmbJkNY38w2T8A.png",
            FreeCompanyFocus.Trials => "https://lds-img.finalfantasyxiv.com/h/P/ydn7ovgdfDc3MocTJV0vP48l4E.png",
            FreeCompanyFocus.Raids => "https://lds-img.finalfantasyxiv.com/h/R/R7mn0-cTBIhT8CcxbtDB-YuPUI.png",
            FreeCompanyFocus.PvP => "https://lds-img.finalfantasyxiv.com/h/D/hXSGh69WuUbiC5NmlZhDyaO2g8.png",
            _ => throw new ArgumentOutOfRangeException(nameof(focus), focus, null)
        };
    }
}