using AutoMapper;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Queries;
using NetStone.Model.Parseables;
using NetStone.Model.Parseables.Search.FreeCompany;

namespace NetStone.Data.AutoMapperProfiles;

public class FreeCompanySearchProfile : Profile
{
    public FreeCompanySearchProfile()
    {
        CreateMap<FreeCompanySearchQuery, Search.FreeCompany.FreeCompanySearchQuery>();

        CreateMap<FreeCompanySearchPage, FreeCompanySearchPageDto>();

        CreateMap<FreeCompanySearchEntry, FreeCompanySearchPageResultDto>();

        CreateMap<IconLayers, FreeCompanyCrestDto>();
    }
}