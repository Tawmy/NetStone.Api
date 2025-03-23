using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Interfaces;

public interface IFreeCompanyCachingServiceV3
{
    #region Free Company

    Task<FreeCompanyDtoV3> CacheFreeCompanyAsync(LodestoneFreeCompany freeCompany);
    Task<FreeCompanyDtoV3?> GetFreeCompanyAsync(int id);
    Task<FreeCompanyDtoV3?> GetFreeCompanyAsync(string lodestoneId);

    #endregion

    #region Free Company Members

    Task<ICollection<FreeCompanyMemberDtoV3>> CacheFreeCompanyMembersAsync(string freeCompanyLodestoneId,
        ICollection<FreeCompanyMembersEntry> freeCompanyMembers);

    Task<(ICollection<FreeCompanyMemberDtoV3> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(int id);

    Task<(ICollection<FreeCompanyMemberDtoV3> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId);

    #endregion
}