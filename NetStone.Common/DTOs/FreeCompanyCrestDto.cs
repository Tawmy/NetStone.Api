namespace NetStone.Common.DTOs;

public record FreeCompanyCrestDto
{
    public string? TopLayer { get; init; }
    public string? MiddleLayer { get; init; }
    public string? BottomLayer { get; init; }
}