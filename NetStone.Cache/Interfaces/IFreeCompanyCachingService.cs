using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Interfaces;

public interface IFreeCompanyCachingService
{
    #region Free Company

    Task<FreeCompanyDto> CacheFreeCompanyAsync(LodestoneFreeCompany freeCompany);
    Task<FreeCompanyDto?> GetFreeCompanyAsync(int id);
    Task<FreeCompanyDto?> GetFreeCompanyAsync(string lodestoneId);
    Task<FreeCompanyDto?> GetFreeCompanyAsync(string name, string world);

    #endregion

    #region Free Company Members

    Task<ICollection<FreeCompanyMemberDto>> CacheFreeCompanyMembersAsync(string freeCompanyLodestoneId,
        ICollection<FreeCompanyMembersEntry> freeCompanyMembers);

    Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(int id);

    Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId);

    #endregion
}