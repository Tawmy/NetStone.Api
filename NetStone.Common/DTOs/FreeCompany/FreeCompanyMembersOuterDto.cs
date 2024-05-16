namespace NetStone.Common.DTOs.FreeCompany;

public record FreeCompanyMembersOuterDto(IEnumerable<FreeCompanyMemberDto> Members, bool Cached, DateTime? LastUpdated);