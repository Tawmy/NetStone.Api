using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs;
using NetStone.Common.Exceptions;
using NetStone.Model.Parseables.Character;
using CharacterClassJob = NetStone.Model.Parseables.Character.ClassJob.CharacterClassJob;

namespace NetStone.Cache.Services;

public class CharacterCachingService(DatabaseContext context, IMapper mapper, CharacterClassJobsService jobsService)
    : ICharacterCachingService
{
    public async Task<CharacterDto> CacheCharacterAsync(string lodestoneId, LodestoneCharacter lodestoneCharacter)
    {
        var character = await context.Characters
            .IncludeBasic()
            .SingleOrDefaultAsync(x => x.Name == lodestoneCharacter.Name && x.Server == lodestoneCharacter.Server);

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

        return mapper.Map<CharacterDto>(character);
    }

    public async Task<CharacterDto?> GetCharacterAsync(int id)
    {
        var character = await context.Characters.IncludeBasic().SingleOrDefaultAsync(x => x.Id == id);
        return character != null ? mapper.Map<CharacterDto>(character) : null;
    }

    public async Task<CharacterDto?> GetCharacterAsync(string lodestoneId)
    {
        var character = await context.Characters.IncludeBasic().SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return character != null ? mapper.Map<CharacterDto>(character) : null;
    }

    public async Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs)
    {
        var character = await context.Characters.Where(x =>
                x.LodestoneId == lodestoneId)
            .FirstOrDefaultAsync();

        if (character is null)
        {
            throw new NotFoundException();
        }

        var dbClassJobs = await context.CharacterClassJobs.Where(x =>
                x.Character.LodestoneId == lodestoneId)
            .ToListAsync();

        dbClassJobs = jobsService.GetCharacterClassJobs(lodestoneClassJobs, dbClassJobs).ToList();

        foreach (var dbClassJob in dbClassJobs.Where(x => x.CharacterId == default))
        {
            // Set FK for new entries
            dbClassJob.CharacterId = character.Id;
        }

        // add new entries to context
        await context.AddRangeAsync(dbClassJobs.Where(x => x.Id == default));

        character.CharacterClassJobsUpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return dbClassJobs.Select(mapper.Map<CharacterClassJobDto>).ToList();
    }

    public async Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)>
        GetCharacterClassJobsAsync(int id)
    {
        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.CharacterClassJobs)
            .FirstOrDefaultAsync();

        if (character == null)
        {
            return (new List<CharacterClassJobDto>(), null);
        }

        var classJobs = character.CharacterClassJobs.Select(mapper.Map<CharacterClassJobDto>);
        return (classJobs.ToList(), character.CharacterClassJobsUpdatedAt);
    }

    public async Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId)
    {
        var character = await context.Characters.Where(x =>
                x.LodestoneId == lodestoneId)
            .Include(x =>
                x.CharacterClassJobs)
            .FirstOrDefaultAsync();

        if (character == null)
        {
            return (new List<CharacterClassJobDto>(), null);
        }

        var classJobs = character.CharacterClassJobs.Select(mapper.Map<CharacterClassJobDto>);
        return (classJobs.ToList(), character.CharacterClassJobsUpdatedAt);
    }
}