using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Data.Interfaces;

public interface IFreeCompanyEvents
{
    Task FreeCompanyRefreshedAsync(FreeCompanyDtoV2 freeCompanyDto);
    Task FreeCompanyRefreshedAsync(FreeCompanyDtoV3 freeCompanyDto);

    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV2 freeCompanyMemberDto);
    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 freeCompanyMemberDto);
}