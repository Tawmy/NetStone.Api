using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Data.Interfaces;

public interface IFreeCompanyEvents
{
    Task FreeCompanyRefreshedAsync(FreeCompanyDtoV3 freeCompanyDto);
    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 freeCompanyMemberDto);
}