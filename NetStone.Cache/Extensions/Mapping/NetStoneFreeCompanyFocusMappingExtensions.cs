using NetStone.Common.Enums;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneFreeCompanyFocusMappingExtensions
{
    public static FreeCompanyFocus ToEnum(this Model.Parseables.FreeCompany.FreeCompanyFocus? source)
    {
        var focus = FreeCompanyFocus.NotSpecified;

        if (source?.HasFocus is not true)
        {
            return focus;
        }

        if (source.RolePlay.IsEnabled)
        {
            focus |= FreeCompanyFocus.RolePlay;
        }

        if (source.Leveling.IsEnabled)
        {
            focus |= FreeCompanyFocus.Leveling;
        }

        if (source.Casual.IsEnabled)
        {
            focus |= FreeCompanyFocus.Casual;
        }

        if (source.Hardcore.IsEnabled)
        {
            focus |= FreeCompanyFocus.Hardcore;
        }

        if (source.Dungeons.IsEnabled)
        {
            focus |= FreeCompanyFocus.Dungeons;
        }

        if (source.Guildhests.IsEnabled)
        {
            focus |= FreeCompanyFocus.Guildhests;
        }

        if (source.Trials.IsEnabled)
        {
            focus |= FreeCompanyFocus.Trials;
        }

        if (source.Raids.IsEnabled)
        {
            focus |= FreeCompanyFocus.Raids;
        }

        if (source.PvP.IsEnabled)
        {
            focus |= FreeCompanyFocus.PvP;
        }

        return focus;
    }
}