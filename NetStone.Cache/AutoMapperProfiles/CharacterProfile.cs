using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character;
using NetStone.StaticData;

namespace NetStone.Cache.AutoMapperProfiles;

internal class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<LodestoneCharacter, Character>()
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()))
            .ForMember(x => x.GrandCompany,
                x => x.MapFrom((y, _) =>
                    Enum.TryParse<GrandCompany>(y.GrandCompanyName, true, out var result)
                        ? result
                        : GrandCompany.None));

        CreateMap<Character, CharacterDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId));
    }
}