using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Common.DTOs.Character;

public record CharacterFreeCompanyDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }
    public required string Link { get; init; }

    public required FreeCompanyCrestDto IconLayers { get; init; }
}