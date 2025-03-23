using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Services;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.Db.Resolvers;

internal class CharacterGearResolver(CharacterGearServiceV2 gearService)
    : IValueResolver<LodestoneCharacter, Character, ICollection<CharacterGear>>
{
    public ICollection<CharacterGear> Resolve(LodestoneCharacter source, Character destination,
        ICollection<CharacterGear> destMember, ResolutionContext context)
    {
        return gearService.GetGear(source.Gear, destination.Gear);
    }
}