using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Interfaces;

public interface IFreeCompanyCachingServiceV2
{
    #region Free Company

    Task<FreeCompanyDtoV2> CacheFreeCompanyAsync(LodestoneFreeCompany freeCompany);
    Task<FreeCompanyDtoV2?> GetFreeCompanyAsync(int id);
    Task<FreeCompanyDtoV2?> GetFreeCompanyAsync(string lodestoneId);

    #endregion

    #region Free Company Members

    Task<ICollection<FreeCompanyMemberDtoV2>> CacheFreeCompanyMembersAsync(string freeCompanyLodestoneId,
        ICollection<FreeCompanyMembersEntry> freeCompanyMembers);

    Task<(ICollection<FreeCompanyMemberDtoV2> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(int id);

    Task<(ICollection<FreeCompanyMemberDtoV2> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId);

    #endregion
}