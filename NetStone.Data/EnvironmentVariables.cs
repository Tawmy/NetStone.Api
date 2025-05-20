namespace NetStone.Data;

/// <summary>
///     Environment variables, to avoid typos.
/// </summary>
internal static class EnvironmentVariables
{
    /// <summary>
    ///     Total amount of minions in the game. Used for calculating percentage collected
    /// </summary>
    public const string FfxivTotalMinions = "FFXIV_TOTAL_MINIONS";

    /// <summary>
    ///     Total amount of mounts in the game. Used for calculating percentage collected
    /// </summary>
    public const string FfxivTotalMounts = "FFXIV_TOTAL_MOUNTS";
}