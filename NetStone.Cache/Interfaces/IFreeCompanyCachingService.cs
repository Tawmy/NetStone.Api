using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;

namespace NetStone.Cache.Interfaces;

public interface IFreeCompanyCachingService
{
    Task<FreeCompanyDto> CacheFreeCompanyAsync(LodestoneFreeCompany freeCompany);
    Task<FreeCompanyDto?> GetFreeCompanyAsync(int id);
    Task<FreeCompanyDto?> GetFreeCompanyAsync(string lodestoneId);
}