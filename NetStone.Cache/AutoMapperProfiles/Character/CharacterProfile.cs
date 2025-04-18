using AutoMapper;
using NetStone.Cache.Db.Resolvers;
using NetStone.Cache.Extensions;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Helpers;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<LodestoneCharacter, Db.Models.Character>()
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()))
            .ForMember(x => x.GrandCompany, x => x.MapFrom<CharacterGrandCompanyResolver>())
            .ForMember(x => x.PvpTeam, x => x.MapFrom(y => y.PvPTeam != null ? y.PvPTeam.Name : null))
            .ForMember(x => x.Race, x => x.MapFrom(y => EnumHelper.ParseFromDisplayString<Race>(y.Race)))
            .ForMember(x => x.Tribe, x => x.MapFrom(y => EnumHelper.ParseFromDisplayString<Tribe>(y.Tribe)))
            .ForMember(x => x.Gender, x => x.MapFrom<CharacterGenderResolver>())
            .ForMember(x => x.Gear, x => x.MapFrom<CharacterGearResolver>())
            .ForMember(x => x.Minions, x => x.Ignore()) // why is this necessary, but ignoring ClassJobs is not?
            .ForMember(x => x.Mounts, x => x.Ignore()); // same here

        CreateMap<Db.Models.Character, CharacterDtoV2>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId))
            .ForMember(x => x.LastUpdated, x => x.MapFrom(y => y.CharacterUpdatedAt))
            .ForMember(x => x.Attributes, x => x.MapFrom<CharacterAttributeResolver>());
    }
}