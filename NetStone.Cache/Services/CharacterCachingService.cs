using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Collectable;
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

        await using var transaction = await context.Database.BeginTransactionAsync();

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
            await context.SaveChangesAsync();

            var classJobs = await context.CharacterClassJobs.Where(x =>
                    x.CharacterLodestoneId == character.LodestoneId)
                .ToListAsync();
            classJobs.ForEach(x => x.CharacterId = character.Id);

            var minions = await context.CharacterMinions.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync();
            minions.ForEach(x => x.CharacterId = character.Id);

            var mounts = await context.CharacterMounts.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync();
            mounts.ForEach(x => x.CharacterId = character.Id);
        }

        character.CharacterUpdatedAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }

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
        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbClassJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        dbClassJobs = jobsService.GetCharacterClassJobs(lodestoneClassJobs, dbClassJobs).ToList();

        if (character is not null)
        {
            foreach (var dbClassJob in dbClassJobs.Where(x => x.CharacterId == null))
            {
                // Set FK for new entries
                dbClassJob.CharacterId = character.Id;
            }
        }

        // add new entries to context
        foreach (var newDbClassJob in dbClassJobs.Where(x => x.Id == default))
        {
            newDbClassJob.CharacterLodestoneId = lodestoneId;
            await context.AddAsync(newDbClassJob);
        }

        if (character is not null)
        {
            character.CharacterClassJobsUpdatedAt = DateTime.UtcNow;
        }

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
        var classJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        var classJobDtos = classJobs.Select(mapper.Map<CharacterClassJobDto>);
        return (classJobDtos.ToList(), null);
    }

    public async Task<ICollection<CharacterMinionDto>> CacheCharacterMinionsAsync(string lodestoneId,
        CharacterCollectable lodestoneMinions)
    {
        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbMinions = await context.CharacterMinions.Where(x => x.CharacterLodestoneId == lodestoneId).ToListAsync();

        // add new Lodestone minions
        var newLodestoneMinions =
            lodestoneMinions.Collectables.Where(x => !dbMinions.Select(y => y.Name).Contains(x.Name));
        var newDbMinions = new List<CharacterMinion>();
        foreach (var newLodestoneMinion in newLodestoneMinions)
        {
            var newDbMinion = mapper.Map<CharacterMinion>(newLodestoneMinion);
            newDbMinion.CharacterLodestoneId = lodestoneId;
            newDbMinion.CharacterId = character?.Id ?? null;
            newDbMinions.AddIfNotNull(newDbMinion);
        }

        await context.CharacterMinions.AddRangeAsync(newDbMinions);
        dbMinions.AddRange(newDbMinions);

        // set FK if necessary
        if (character is not null)
        {
            var dbMinionsWithoutFk = dbMinions.Where(x => x.CharacterId is null);
            foreach (var dbMinionWithoutFk in dbMinionsWithoutFk)
            {
                dbMinionWithoutFk.CharacterId = character.Id;
            }
        }

        if (character is not null)
        {
            character.CharacterMinionsUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return dbMinions.Select(mapper.Map<CharacterMinionDto>).ToList();
    }

    public async Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(int id)
    {
        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Minions)
            .FirstOrDefaultAsync();

        if (character == null)
        {
            return (new List<CharacterMinionDto>(), null);
        }

        var minions = character.Minions.Select(mapper.Map<CharacterMinionDto>);
        return (minions.ToList(), character.CharacterMinionsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(
        string lodestoneId)
    {
        var minions = await context.CharacterMinions.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        var minionDtos = minions.Select(mapper.Map<CharacterMinionDto>);
        return (minionDtos.ToList(), null);
    }

    public async Task<ICollection<CharacterMountDto>> CacheCharacterMountsAsync(string lodestoneId,
        CharacterCollectable lodestoneMounts)
    {
        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbMounts = await context.CharacterMounts.Where(x => x.CharacterLodestoneId == lodestoneId).ToListAsync();

        // add new Lodestone mounts
        var newLodestoneMounts =
            lodestoneMounts.Collectables.Where(x => !dbMounts.Select(y => y.Name).Contains(x.Name));
        var newDbMounts = new List<CharacterMount>();
        foreach (var newLodestoneMount in newLodestoneMounts)
        {
            var newDbMount = mapper.Map<CharacterMount>(newLodestoneMount);
            newDbMount.CharacterLodestoneId = lodestoneId;
            newDbMount.CharacterId = character?.Id ?? null;
            newDbMounts.AddIfNotNull(newDbMount);
        }

        await context.CharacterMounts.AddRangeAsync(newDbMounts);
        dbMounts.AddRange(newDbMounts);

        // set FK if necessary
        if (character is not null)
        {
            var dbMountsWithoutFk = dbMounts.Where(x => x.CharacterId is null);
            foreach (var dbMountWithoutFk in dbMountsWithoutFk)
            {
                dbMountWithoutFk.CharacterId = character.Id;
            }
        }

        if (character is not null)
        {
            character.CharacterMountsUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return dbMounts.Select(mapper.Map<CharacterMountDto>).ToList();
    }

    public async Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(int id)
    {
        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Mounts)
            .FirstOrDefaultAsync();

        if (character == null)
        {
            return (new List<CharacterMountDto>(), null);
        }

        var mounts = character.Mounts.Select(mapper.Map<CharacterMountDto>);
        return (mounts.ToList(), character.CharacterMountsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(
        string lodestoneId)
    {
        var mounts = await context.CharacterMounts.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        var mountDtos = mounts.Select(mapper.Map<CharacterMountDto>);
        return (mountDtos.ToList(), null);
    }
}