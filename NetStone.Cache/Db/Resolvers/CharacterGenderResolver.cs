using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.Db.Resolvers;

public class CharacterGenderResolver : IValueResolver<LodestoneCharacter, Character, Gender>
{
    public Gender Resolve(LodestoneCharacter source, Character destination, Gender destMember,
        ResolutionContext context)
    {
        return source.Gender switch
        {
            LodestoneCharacter.MaleChar => Gender.Male,
            LodestoneCharacter.FemaleChar => Gender.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source.Gender, "Gender not recognized.")
        };
    }
}