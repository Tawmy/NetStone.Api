using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.FreeCompany;

namespace NetStone.Cache.Db.Resolvers;

public class FreeCompanyGrandCompanyResolver : IValueResolver<LodestoneFreeCompany, FreeCompany, GrandCompany>
{
    public GrandCompany Resolve(LodestoneFreeCompany source, FreeCompany destination, GrandCompany destMember,
        ResolutionContext context)
    {
        return source.GrandCompany.ToLowerInvariant() switch
        {
            "maelstrom" => GrandCompany.Maelstrom,
            "order of the twin adder" => GrandCompany.OrderOfTheTwinAdder,
            "immortal flames" => GrandCompany.ImmortalFlames,
            _ => GrandCompany.NoAffiliation
        };
    }
}