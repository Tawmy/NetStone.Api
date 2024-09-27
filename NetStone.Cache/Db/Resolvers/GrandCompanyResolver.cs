using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.Db.Resolvers;

public class GrandCompanyResolver : IValueResolver<LodestoneCharacter, Character, GrandCompany>
{
    public GrandCompany Resolve(LodestoneCharacter source, Character destination, GrandCompany destMember,
        ResolutionContext context)
    {
        return source.GrandCompanyName.ToLowerInvariant() switch
        {
            "maelstrom" => GrandCompany.Maelstrom,
            "adder" => GrandCompany.OrderOfTheTwinAdder,
            "flames" => GrandCompany.ImmortalFlames,
            _ => GrandCompany.NoAffiliation
        };
    }
}