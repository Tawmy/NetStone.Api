using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.AutoMapperProfiles;

public class CharacterMinionsProfile : Profile
{
    public CharacterMinionsProfile()
    {
        CreateMap<CharacterCollectableEntry, CharacterMinion>();

        CreateMap<CharacterMinion, CharacterMinionDto>();
    }
}