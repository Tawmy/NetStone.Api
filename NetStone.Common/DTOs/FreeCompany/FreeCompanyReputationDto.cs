using NetStone.Common.Enums;

namespace NetStone.Common.DTOs.FreeCompany;

public record FreeCompanyReputationDto(GrandCompany GrandCompany, string Rank, short Progress);