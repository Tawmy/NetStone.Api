namespace NetStone.Cache.Interfaces;

internal interface IUpdatable
{
    /// <summary>
    ///     Automatically set when database entry is first created.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Automatically set whenever database entry is modified.
    /// </summary>
    DateTime? UpdatedAt { get; set; }
}