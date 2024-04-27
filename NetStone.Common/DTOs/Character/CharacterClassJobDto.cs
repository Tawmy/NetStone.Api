using NetStone.Common.Enums;

namespace NetStone.Common.DTOs.Character;

public record CharacterClassJobDto
{
    public required ClassJob ClassJob { get; init; }

    /// <summary>
    ///     Applicable to jobs with class. True for both MRD and WAR if WAR unlocked, for example.
    /// </summary>
    public required bool IsJobUnlocked { get; init; }

    public required short Level { get; init; }

    public required int ExpCurrent { get; init; }
    public required int ExpMax { get; init; }
    public required int ExpToGo { get; init; }

    public required bool IsSpecialized { get; init; }
}