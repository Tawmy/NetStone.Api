using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Db.Resolvers;
using NetStone.Cache.Extensions;
using NetStone.Common.DTOs;
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
                        : GrandCompany.None))
            .ForMember(x => x.Gear, x => x.MapFrom<CharacterGearResolver>());

        CreateMap<Character, CharacterDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId))
            .ForMember(x => x.LastUpdated, x => x.MapFrom(y => y.CharacterUpdatedAt));
    }
}