using AutoMapper;
using NetStone.Cache.Db.Resolvers;
using NetStone.Cache.Extensions;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.AutoMapperProfiles.Character;

internal class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<LodestoneCharacter, Db.Models.Character>()
            .ForMember(x => x.ActiveClassJob, x => x.MapFrom(y => y.GetActiveClassJob()))
            .ForMember(x => x.GrandCompany,
                x => x.MapFrom((y, _) =>
                    Enum.TryParse<GrandCompany>(y.GrandCompanyName, true, out var result)
                        ? result
                        : GrandCompany.NoAffiliation))
            .ForMember(x => x.Gear, x => x.MapFrom<CharacterGearResolver>())
            .ForMember(x => x.Minions, x => x.Ignore()) // why is this necessary, but ignoring ClassJobs is not?
            .ForMember(x => x.Mounts, x => x.Ignore()); // same here

        CreateMap<Db.Models.Character, CharacterDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.LodestoneId))
            .ForMember(x => x.LastUpdated, x => x.MapFrom(y => y.CharacterUpdatedAt))
            .ForMember(x => x.CachedFreeCompany, x => x.MapFrom(y => y.FullFreeCompany));
    }
}