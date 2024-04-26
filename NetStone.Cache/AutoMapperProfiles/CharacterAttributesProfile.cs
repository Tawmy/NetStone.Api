using AutoMapper;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles;

public class CharacterAttributesProfile : Profile
{
    public CharacterAttributesProfile()
    {
        CreateMap<CharacterAttributes, Db.Models.CharacterAttributes>();

        CreateMap<Db.Models.CharacterAttributes, CharacterAttributesDto>();
    }
}