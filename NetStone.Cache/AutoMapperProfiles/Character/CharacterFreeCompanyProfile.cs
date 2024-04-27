using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterFreeCompanyProfile : Profile
{
    public CharacterFreeCompanyProfile()
    {
        CreateMap<FreeCompanySocialGroup, CharacterFreeCompany>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.LodestoneId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.TopLayer, x => x.MapFrom(y => y.IconLayers.TopLayer))
            .ForMember(x => x.MiddleLayer, x => x.MapFrom(y => y.IconLayers.MiddleLayer))
            .ForMember(x => x.BottomLayer, x => x.MapFrom(y => y.IconLayers.BottomLayer));

        CreateMap<CharacterFreeCompany, CharacterFreeCompanyDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId))
            .ForMember(x => x.IconLayers,
                x => x.MapFrom(y => new FreeCompanyCrestDto
                    { TopLayer = y.TopLayer, MiddleLayer = y.MiddleLayer, BottomLayer = y.BottomLayer }));
    }
}