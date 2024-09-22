using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.AutoMapperProfiles.Character;

public class CharacterMinionProfile : Profile
{
    public CharacterMinionProfile()
    {
        CreateMap<CharacterCollectableEntry, CharacterMinion>();

        CreateMap<CharacterMinion, CharacterMinionDto>();
    }
}