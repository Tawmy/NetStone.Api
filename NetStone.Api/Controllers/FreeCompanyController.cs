using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Api.Exceptions;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;
using NetStone.Model.Parseables.Search.FreeCompany;
using NetStone.Search.FreeCompany;

namespace NetStone.Api.Controllers;

/// <summary>
///     Free Company controller. Parses Lodestone for Free Company data.
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize]
public class FreeCompanyController : ControllerBase
{
    private readonly IFreeCompanyService _freeCompanyService;

    /// <inheritdoc />
    public FreeCompanyController(IFreeCompanyService freeCompanyService)
    {
        _freeCompanyService = freeCompanyService;
    }

    /// <summary>
    ///     Search for free company with provided search query.
    /// </summary>
    /// <param name="query">Search query, Name and World are needed.</param>
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Results returned from Lodestone.</returns>
    [HttpPost("Search")]
    public async Task<ActionResult<FreeCompanySearchPage>> SearchAsync(FreeCompanySearchQuery query, int page = 1)
    {
        try
        {
            return await _freeCompanyService.SearchFreeCompanyAsync(query, page);
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
    /// <returns>Parsed free company data.</returns>
    [HttpGet("{lodestoneId}")]
    public async Task<ActionResult<LodestoneFreeCompany>> GetAsync(string lodestoneId)
    {
        try
        {
            return await _freeCompanyService.GetFreeCompanyAsync(lodestoneId);
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
    /// <param name="page">Which page of the paginated results to return.</param>
    /// <returns>Free company members.</returns>
    [HttpGet("Members/{lodestoneId}")]
    public async Task<ActionResult<FreeCompanyMembers>> GetMembersAsync(string lodestoneId, int page = 1)
    {
        try
        {
            return await _freeCompanyService.GetFreeCompanyMembersAsync(lodestoneId, page);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}