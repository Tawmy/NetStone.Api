using AutoMapper;
using NetStone.Common.DTOs;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character;

namespace NetStone.Api.AutoMapperProfiles;

internal class LodestoneCharacterProfile : Profile
{
    public LodestoneCharacterProfile()
    {
        CreateMap<LodestoneCharacter, CharacterDto>()
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()));
    }
}