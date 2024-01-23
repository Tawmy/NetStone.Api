using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetStone.Api.Interfaces;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Search.Character;
using NetStone.Search.Character;

namespace NetStone.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpPost("Search")]
    public async Task<CharacterSearchPage?> SearchAsync(CharacterSearchQuery searchEntry)
    {
        return await _characterService.SearchCharacterAsync(searchEntry);
    }

    [HttpGet("{lodestoneId}")]
    public async Task<LodestoneCharacter?> GetAsync(string lodestoneId)
    {
        return await _characterService.GetCharacterAsync(lodestoneId);
    }
}