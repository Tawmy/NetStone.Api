using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;

namespace NetStone.Data.Interfaces;

/// <summary>
///     Data service for free company data.
/// </summary>
public interface IFreeCompanyServiceV3
{
    /// <summary>
    ///     Search for free company with provided search query.
    /// </summary>
    /// <param name="query">
    ///     Search query, only <see cref="FreeCompanySearchQuery.Name" /> and
    ///     <see cref="FreeCompanySearchQuery.World" /> are needed.
    /// </param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Instance of <see cref="FreeCompanySearchPageDto" /> with the results returned from the Lodestone.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<FreeCompanySearchPageDto> SearchFreeCompanyAsync(FreeCompanySearchQuery query, int page);

    /// <summary>
    ///     Get character with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone character ID. Use <see cref="SearchFreeCompanyAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached free company, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
    /// <returns>Parsed free company data.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<FreeCompanyDtoV3> GetFreeCompanyAsync(string lodestoneId, int? maxAge, bool useFallback);

    /// <summary>
    ///     Get a free company's members.
    /// </summary>
    /// <param name="lodestoneId">Lodestone free company ID. Use Use <see cref="SearchFreeCompanyAsync" /> first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached free company members, in minutes. If older, they will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <param name="useFallback">If true, API will return cached data if Lodestone unavailable or parsing failed.</param>
    /// <returns>Free company members.</returns>
    /// <exception cref="NotFoundException"></exception>
    Task<FreeCompanyMembersOuterDtoV3> GetFreeCompanyMembersAsync(string lodestoneId, int? maxAge, bool useFallback);
}