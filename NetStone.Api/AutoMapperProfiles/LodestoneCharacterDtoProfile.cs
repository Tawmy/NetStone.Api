using AutoMapper;
using NetStone.Api.DTOs;
using NetStone.Api.Extensions;
using NetStone.Model.Parseables.Character;

namespace NetStone.Api.AutoMapperProfiles;

internal class LodestoneCharacterDtoProfile : Profile
{
    public LodestoneCharacterDtoProfile()
    {
        CreateMap<LodestoneCharacter, LodestoneCharacterDto>()
            .ForMember(x => x.Character, x => x.MapFrom(y => y))
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()));
    }
}