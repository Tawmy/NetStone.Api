using AutoMapper;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Queries;
using NetStone.Model.Parseables.Search.Character;

namespace NetStone.Data.AutoMapperProfiles;

public class CharacterSearchProfile : Profile
{
    public CharacterSearchProfile()
    {
        CreateMap<CharacterSearchQuery, Search.Character.CharacterSearchQuery>();

        CreateMap<CharacterSearchPage, CharacterSearchPageDto>();

        CreateMap<CharacterSearchEntry, CharacterSearchPageResultDto>();
    }
}