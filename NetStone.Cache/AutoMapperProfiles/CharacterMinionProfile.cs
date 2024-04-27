using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.AutoMapperProfiles;

public class CharacterMinionProfile : Profile
{
    public CharacterMinionProfile()
    {
        CreateMap<CharacterCollectableEntry, CharacterMinion>();

        CreateMap<CharacterMinion, CharacterMinionDto>();
    }
}