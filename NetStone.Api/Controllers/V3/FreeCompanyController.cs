using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Common.Exceptions;
using NetStone.Common.Queries;
using NetStone.Data.Interfaces;

namespace NetStone.Api.Controllers.V3;

/// <summary>
///     Free Company controller. Parses Lodestone for Free Company data.
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize]
[ApiVersion(3)]
[Obsolete("This API version is deprecated. Please use version 4 instead.")]
public class FreeCompanyController(IFreeCompanyServiceV3 freeCompanyService) : ControllerBase
{
    /// <summary>
    ///     Search for free company with provided search query.
    /// </summary>
    /// <param name="query">Search query, Name, and World are needed.</param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Results returned from Lodestone.</returns>
    [HttpPost("Search")]
    public async Task<ActionResult<FreeCompanySearchPageDto>> SearchAsync(FreeCompanySearchQuery query,
        int page = 1)
    {
        try
        {
            return await freeCompanyService.SearchFreeCompanyAsync(query, page);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get free company with the given ID from the Lodestone.
    /// </summary>
    /// <param name="lodestoneId">Lodestone free company ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached free company, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>Parsed free company data.</returns>
    [HttpGet("{lodestoneId}")]
    [ProducesResponseType<FreeCompanyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FreeCompanyDto>> GetAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback = FallbackTypeV3.None)
    {
        try
        {
            return await freeCompanyService.GetFreeCompanyAsync(lodestoneId, maxAge, useFallback);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get free company with the given name and world <b>from cache</b>.
    /// </summary>
    /// <param name="name">Free company name. Must be exact match, but is case insensitive.</param>
    /// <param name="world">World, case insensitive.</param>
    /// <returns>DTO containing the free company and some goodie properties.</returns>
    [HttpGet("ByName/{name}/{world}")]
    [ProducesResponseType<FreeCompanyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FreeCompanyDto>> GetByNameAsync(string name, string world)
    {
        try
        {
            return await freeCompanyService.GetFreeCompanyByNameAsync(name, world);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    ///     Get a free company's members.
    /// </summary>
    /// <param name="lodestoneId">Lodestone free company ID. Use Search endpoint first if unknown.</param>
    /// <param name="maxAge">
    ///     Optional maximum age of cached free company members, in minutes. If older, it will be refreshed from the
    ///     Lodestone.
    /// </param>
    /// <remarks>
    ///     If free company was never cached using <see cref="GetAsync" />,
    ///     <see cref="FreeCompanyMembersOuterDto.LastUpdated" />cannot be set. Its value will be null as a result. In this
    ///     case, if <paramref name="maxAge" /> is set to ANY value, the data will be refreshed. If free company was cached at
    ///     least once and the value can be saved, <paramref name="maxAge" /> applies as expected.
    /// </remarks>
    /// <param name="useFallback">
    ///     API may return cached data if Lodestone unavailable or parsing failed.
    ///     Set to Http to handle HttpRequestExceptions (eg. when the Lodestone is down),
    ///     and to Any to handle any exception including errors in the parser.
    ///     Do note that exceptions in the parser may have to be fixed manually and will not resolve themselves.
    /// </param>
    /// <returns>Free company members.</returns>
    [HttpGet("Members/{lodestoneId}")]
    [ProducesResponseType<FreeCompanyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FreeCompanyMembersOuterDto>> GetMembersAsync(string lodestoneId, int? maxAge,
        FallbackTypeV3 useFallback = FallbackTypeV3.None)
    {
        try
        {
            return await freeCompanyService.GetFreeCompanyMembersAsync(lodestoneId, maxAge, useFallback);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}