using AutoMapper;
using NetStone.Cache.Extensions;
using NetStone.Data.LegacyDtos;
using NetStone.Model.Parseables.Character;

namespace NetStone.Data.AutoMapperProfiles;

internal class LodestoneCharacterDtoProfile : Profile
{
    public LodestoneCharacterDtoProfile()
    {
        CreateMap<LodestoneCharacter, LegacyLodestoneCharacterDto>()
            .ForMember(x => x.Character, x => x.MapFrom(y => y))
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()));
    }
}