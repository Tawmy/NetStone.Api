using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.AutoMapperProfiles;

public class CharacterMountProfile : Profile
{
    public CharacterMountProfile()
    {
        CreateMap<CharacterCollectableEntry, CharacterMount>();

        CreateMap<CharacterMount, CharacterMountDto>();
    }
}