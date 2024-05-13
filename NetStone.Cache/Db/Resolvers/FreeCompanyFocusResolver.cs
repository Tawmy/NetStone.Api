using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.FreeCompany;
using FreeCompanyFocus = NetStone.Common.Enums.FreeCompanyFocus;

namespace NetStone.Cache.Db.Resolvers;

public class FreeCompanyFocusResolver : IValueResolver<LodestoneFreeCompany, FreeCompany, FreeCompanyFocus>
{
    public FreeCompanyFocus Resolve(LodestoneFreeCompany source, FreeCompany destination, FreeCompanyFocus destMember,
        ResolutionContext context)
    {
        var focus = FreeCompanyFocus.None;

        if (source.Focus is null)
        {
            return focus;
        }

        if (source.Focus?.HasFocus is not true)
        {
            return focus;
        }

        if (source.Focus.RolePlay.IsEnabled)
        {
            focus |= FreeCompanyFocus.RolePlaying;
        }

        if (source.Focus.Leveling.IsEnabled)
        {
            focus |= FreeCompanyFocus.Leveling;
        }

        if (source.Focus.Casual.IsEnabled)
        {
            focus |= FreeCompanyFocus.Casual;
        }

        if (source.Focus.Hardcore.IsEnabled)
        {
            focus |= FreeCompanyFocus.Hardcore;
        }

        if (source.Focus.Dungeons.IsEnabled)
        {
            focus |= FreeCompanyFocus.Dungeons;
        }

        if (source.Focus.Guildhests.IsEnabled)
        {
            focus |= FreeCompanyFocus.Guildhests;
        }

        if (source.Focus.Trials.IsEnabled)
        {
            focus |= FreeCompanyFocus.Trials;
        }

        if (source.Focus.Raids.IsEnabled)
        {
            focus |= FreeCompanyFocus.Raids;
        }

        if (source.Focus.PvP.IsEnabled)
        {
            focus |= FreeCompanyFocus.PvP;
        }

        return focus;
    }
}