using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Data.Interfaces;

public interface IFreeCompanyEvents
{
    Task FreeCompanyRefreshedAsync(FreeCompanyDto freeCompanyDto);
    Task FreeCompanyRefreshedAsync(FreeCompanyDtoV3 freeCompanyDto);

    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDto freeCompanyMemberDto);
    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDtoV3 freeCompanyMemberDto);
}