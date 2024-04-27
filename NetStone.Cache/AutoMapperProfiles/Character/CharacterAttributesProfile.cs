using AutoMapper;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterAttributesProfile : Profile
{
    public CharacterAttributesProfile()
    {
        CreateMap<CharacterAttributes, Db.Models.CharacterAttributes>();

        CreateMap<Db.Models.CharacterAttributes, CharacterAttributesDto>();
    }
}