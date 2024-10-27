using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Data.Interfaces;

public interface IFreeCompanyEvents
{
    Task FreeCompanyRefreshedAsync(FreeCompanyDto freeCompanyDto);
    Task FreeCompanyMembersRefreshedAsync(FreeCompanyMembersOuterDto freeCompanyMemberDto);
}