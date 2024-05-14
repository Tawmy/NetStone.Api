using AutoMapper;
using NetStone.Cache.Db.Resolvers;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.FreeCompany;

namespace NetStone.Cache.AutoMapperProfiles.FreeCompany;

public class FreeCompanyProfile : Profile
{
    public FreeCompanyProfile()
    {
        CreateMap<LodestoneFreeCompany, Db.Models.FreeCompany>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.LodestoneId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.CrestTop, x => x.MapFrom(y => y.CrestLayers.TopLayer))
            .ForMember(x => x.CrestMiddle, x => x.MapFrom(y => y.CrestLayers.MiddleLayer))
            .ForMember(x => x.CrestBottom, x => x.MapFrom(y => y.CrestLayers.BottomLayer))
            .ForMember(x => x.GrandCompany, x => x.MapFrom((y, _) =>
                Enum.TryParse<GrandCompany>(y.GrandCompany, true, out var result)
                    ? result
                    : GrandCompany.NoAffiliation))
            .ForMember(x => x.EstateName, x => x.MapFrom(y => y.Estate != null ? y.Estate.Name : null))
            .ForMember(x => x.EstateGreeting, x => x.MapFrom(y => y.Estate != null ? y.Estate.Greeting : null))
            .ForMember(x => x.EstatePlot, x => x.MapFrom(y => y.Estate != null ? y.Estate.Plot : null))
            .ForMember(x => x.Focus, x => x.MapFrom<FreeCompanyFocusResolver>())
            .ForMember(x => x.MaelstromRank, x => x.MapFrom(y => y.Reputation.Maelstrom.Rank))
            .ForMember(x => x.MaelstromProgress, x => x.MapFrom(y => y.Reputation.Maelstrom.Progress))
            .ForMember(x => x.TwinAdderRank, x => x.MapFrom(y => y.Reputation.Adders.Rank))
            .ForMember(x => x.TwinAdderProgress, x => x.MapFrom(y => y.Reputation.Adders.Progress))
            .ForMember(x => x.ImmortalFlamesRank, x => x.MapFrom(y => y.Reputation.Flames.Rank))
            .ForMember(x => x.ImmortalFlamesProgress, x => x.MapFrom(y => y.Reputation.Flames.Progress));

        CreateMap<Db.Models.FreeCompany, FreeCompanyDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId))
            .ForMember(x => x.CrestLayers,
                x => x.MapFrom(y => new FreeCompanyCrestDto(y.CrestTop, y.CrestMiddle, y.CrestBottom)))
            .ForMember(x => x.Estate,
                x => x.MapFrom(y =>
                    string.IsNullOrEmpty(y.EstateName) || string.IsNullOrEmpty(y.EstateGreeting)
                        ? null
                        : new FreeCompanyEstateDto(y.EstateName, y.EstateGreeting, y.EstatePlot)))
            .ForMember(x => x.Focus, x => x.MapFrom<FreeCompanyFocusDtoResolver>())
            .ForMember(x => x.LastUpdated, x => x.MapFrom(y => y.FreeCompanyUpdatedAt))
            .ForMember(x => x.Reputation, x => x.MapFrom<FreeCompanyReputationResolver>());
    }
}