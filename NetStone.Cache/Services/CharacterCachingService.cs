using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs;
using NetStone.Common.Interfaces;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.Services;

public class CharacterCachingService(DatabaseContext context, IMapper mapper) : ICharacterCachingService
{
    public async Task CacheCharacterAsync(LodestoneCharacter lodestoneCharacter, string lodestoneId)
    {
        var character = await context.Characters.SingleOrDefaultAsync(x =>
            x.Name == lodestoneCharacter.Name && x.Server == lodestoneCharacter.Server);

        if (character != null)
        {
            mapper.Map(lodestoneCharacter, character);
            context.Entry(character).State = EntityState.Modified;
        }
        else
        {
            character = mapper.Map<Character>(lodestoneCharacter);
            character.LodestoneId = lodestoneId;
            await context.Characters.AddAsync(character);
        }

        await context.SaveChangesAsync();
    }

    public async Task<CharacterDto?> GetCharacterAsync(int id)
    {
        var character = await context.Characters.SingleOrDefaultAsync(x => x.Id == id);
        return character != null ? mapper.Map<CharacterDto>(character) : null;
    }

    public async Task<CharacterDto?> GetCharacterAsync(string lodestoneId)
    {
        var character = await context.Characters.SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return character != null ? mapper.Map<CharacterDto>(character) : null;
    }
}