using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;

namespace NetStone.Cache.Db.Resolvers;

public class
    FreeCompanyReputationResolver : IValueResolver<FreeCompany, FreeCompanyDto, IEnumerable<FreeCompanyReputationDto>>
{
    public IEnumerable<FreeCompanyReputationDto> Resolve(FreeCompany source, FreeCompanyDto destination,
        IEnumerable<FreeCompanyReputationDto> destMember, ResolutionContext context)
    {
        return new List<FreeCompanyReputationDto>
        {
            new(GrandCompany.Maelstrom, source.MaelstromRank, source.MaelstromProgress),
            new(GrandCompany.OrderOfTheTwinAdder, source.TwinAdderRank,
                source.TwinAdderProgress),
            new(GrandCompany.ImmortalFlames, source.ImmortalFlamesRank, source.ImmortalFlamesProgress)
        };
    }
}