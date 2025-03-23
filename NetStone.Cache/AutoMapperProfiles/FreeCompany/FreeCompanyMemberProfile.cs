using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.AutoMapperProfiles.FreeCompany;

public class FreeCompanyMemberProfile : Profile
{
    public FreeCompanyMemberProfile()
    {
        CreateMap<FreeCompanyMembersEntry, FreeCompanyMember>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.CharacterLodestoneId, x => x.MapFrom(y => y.Id));

        CreateMap<FreeCompanyMember, FreeCompanyMemberDtoV2>()
            .ForMember(x => x.LodestoneId, x => x.MapFrom(y => y.CharacterLodestoneId))
            .ForMember(x => x.CachedCharacter, x => x.MapFrom(y => y.FullCharacter));
    }
}