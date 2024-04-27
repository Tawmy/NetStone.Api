using AutoMapper;
using NetStone.Common.DTOs;
using NetStone.Model.Parseables.Character.Gear;
using CharacterGear = NetStone.Cache.Db.Models.CharacterGear;

namespace NetStone.Cache.AutoMapperProfiles;

internal class CharacterGearProfile : Profile
{
    public CharacterGearProfile()
    {
        CreateMap<GearEntry, CharacterGear>()
            .ForMember(x => x.Materia1, x => x.MapFrom(y => y.Materia.ElementAtOrDefault(0)))
            .ForMember(x => x.Materia2, x => x.MapFrom(y => y.Materia.ElementAtOrDefault(1)))
            .ForMember(x => x.Materia3, x => x.MapFrom(y => y.Materia.ElementAtOrDefault(2)))
            .ForMember(x => x.Materia4, x => x.MapFrom(y => y.Materia.ElementAtOrDefault(3)))
            .ForMember(x => x.Materia5, x => x.MapFrom(y => y.Materia.ElementAtOrDefault(4)));

        CreateMap<SoulcrystalEntry, CharacterGear>();

        CreateMap<CharacterGear, CharacterGearDto>()
            .ForMember(x => x.Materia,
                x => x.MapFrom(y =>
                    new[] { y.Materia1, y.Materia2, y.Materia3, y.Materia4, y.Materia5 }.Where(z => z != null)));
    }
}