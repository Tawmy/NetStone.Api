using AutoMapper;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterAttributesProfile : Profile
{
    public CharacterAttributesProfile()
    {
        CreateMap<CharacterAttributes, Db.Models.CharacterAttributes>();
    }
}