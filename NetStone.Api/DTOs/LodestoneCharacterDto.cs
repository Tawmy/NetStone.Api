using NetStone.Api.Extensions;
using NetStone.Model.Parseables.Character;
using NetStone.StaticData;

namespace NetStone.Api.DTOs;

/// <summary>
///     Character DTO. Goodie properties are on the root level.
/// </summary>
public class LodestoneCharacterDto
{
    /// <summary>
    ///     NetStone chracter entity.
    /// </summary>
    public required LodestoneCharacter Character { get; set; }

    /// <summary>
    ///     Active job. Not included in <see cref="LodestoneCharacter" /> and thus implemented through
    ///     <see cref="LodestoneCharacterExtension.GetActiveClassJob" />.
    /// </summary>
    public required ClassJob ActiveClassJob { get; set; }
}