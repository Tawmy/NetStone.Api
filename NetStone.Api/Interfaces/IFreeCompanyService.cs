using NetStone.Api.Controllers;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Search.FreeCompany;

namespace NetStone.Api.Interfaces;

/// <summary>
///     Controller service for <see cref="FreeCompanyController" />.
/// </summary>
public interface IFreeCompanyService
{
    /// <summary>
    ///     Search for free company with provided search query.
    /// </summary>
    /// <param name="query">
    ///     Search query, only <see cref="FreeCompanySearchQuery.Name" /> and
    ///     <see cref="FreeCompanySearchQuery.World" /> are needed.
    /// </param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Instance of <see cref="FreeCompanySearchPage" /> with the results returned from the Lodestone.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<FreeCompanySearchPage> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchFreeCompanyAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached free company, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <returns>Parsed free company data.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<FreeCompanyDto> GetFreeCompanyAsync(string lodestoneId, int? maxAge);

    /// <summary>
    ///     Get a free company's members.
    /// </summary>
    /// <param name="lodestoneId">Lodestone free company ID. Use Use <see cref="SearchFreeCompanyAsync" /> first if unknown.</param>
    /// <param name="page"></param>
    /// <returns>Free company members.</returns>
    /// <exception cref="NotFoundException">Which page of the paginated results to return.</exception>
    Task<FreeCompanyMembers> GetFreeCompanyMembersAsync(string lodestoneId, int page);
}