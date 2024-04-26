namespace NetStone.Common.DTOs;

public record CharacterFreeCompanyDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }
    public required string Link { get; set; }

    public required FreeCompanyCrestDto IconLayers { get; init; }
}