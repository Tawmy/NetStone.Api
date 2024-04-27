using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.AutoMapperProfiles.Character;

public class CharacterMountProfile : Profile
{
    public CharacterMountProfile()
    {
        CreateMap<CharacterCollectableEntry, CharacterMount>();

        CreateMap<CharacterMount, CharacterMountDto>();
    }
}