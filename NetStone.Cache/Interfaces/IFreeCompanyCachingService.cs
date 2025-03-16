using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Interfaces;

public interface IFreeCompanyCachingService
{
    #region Free Company

    Task<FreeCompanyDtoV3> CacheFreeCompanyAsync(LodestoneFreeCompany freeCompany);
    Task<FreeCompanyDtoV3?> GetFreeCompanyAsync(int id);
    Task<FreeCompanyDtoV3?> GetFreeCompanyAsync(string lodestoneId);

    #endregion

    #region Free Company Members

    Task<ICollection<FreeCompanyMemberDto>> CacheFreeCompanyMembersAsync(string freeCompanyLodestoneId,
        ICollection<FreeCompanyMembersEntry> freeCompanyMembers);

    Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(int id);

    Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId);

    #endregion
}