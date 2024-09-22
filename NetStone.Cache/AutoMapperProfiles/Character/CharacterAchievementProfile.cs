using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character.Achievement;

namespace NetStone.Cache.AutoMapperProfiles.Character;

public class CharacterAchievementProfile : Profile
{
    public CharacterAchievementProfile()
    {
        CreateMap<CharacterAchievementEntry, CharacterAchievement>()
            .ForMember(x => x.AchievementId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Id, x => x.Ignore());

        CreateMap<CharacterAchievement, CharacterAchievementDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.AchievementId));
    }
}