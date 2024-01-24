using NetStone.Model.Parseables.Character;
using NetStone.StaticData;

namespace NetStone.Api.DTOs;

public class LodestoneCharacterDto
{
    public required LodestoneCharacter Character { get; set; }
    public required ClassJob ActiveClassJob { get; set; }
}