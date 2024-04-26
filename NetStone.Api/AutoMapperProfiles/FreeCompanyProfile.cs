using AutoMapper;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables;
using NetStone.Model.Parseables.Character;

namespace NetStone.Api.AutoMapperProfiles;

internal class FreeCompanyProfile : Profile
{
    public FreeCompanyProfile()
    {
        CreateMap<FreeCompanySocialGroup, CharacterFreeCompanyDto>();

        CreateMap<IconLayers, FreeCompanyCrestDto>();
    }
}