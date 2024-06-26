using AutoMapper;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character.ClassJob;
using CharacterClassJob = NetStone.Cache.Db.Models.CharacterClassJob;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterClassJobProfile : Profile
{
    public CharacterClassJobProfile()
    {
        CreateMap<ClassJobEntry, CharacterClassJob>();

        CreateMap<CharacterClassJob, CharacterClassJobDto>();

        // TODO what
        CreateMap<CharacterClassJob, CharacterClassJob>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.CharacterId, x => x.Ignore())
            .ForMember(x => x.Character, x => x.Ignore());
    }
}