using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.Collectable;
using CharacterClassJob = NetStone.Model.Parseables.Character.ClassJob.CharacterClassJob;

namespace NetStone.Cache.Services;

internal class CharacterCachingServiceV2(
    DatabaseContext context,
    IAutoMapperService mapper,
    CharacterClassJobsServiceV2 jobsService)
    : ICharacterCachingServiceV2
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterCachingServiceV2));

    public async Task<CharacterDtoV2> CacheCharacterAsync(string lodestoneId, LodestoneCharacter lodestoneCharacter)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters
            .IncludeBasic()
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);

        await using var transaction = await context.Database.BeginTransactionAsync();

        if (character is not null)
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

            var achievements = await context.CharacterAchievements.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync();
            achievements.ForEach(x => x.CharacterId = character.Id);

            var freeCompanyMembers = await context.FreeCompanyMembers.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync();
            freeCompanyMembers.ForEach(x => x.FullCharacterId = character.Id);
        }

        character.CharacterUpdatedAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return mapper.Map<CharacterDtoV2>(character);
    }

    public async Task<CharacterDtoV2?> GetCharacterAsync(int id)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.Id == id);
        return character is not null ? mapper.Map<CharacterDtoV2>(character) : null;
    }

    public async Task<CharacterDtoV2?> GetCharacterAsync(string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return character is not null ? mapper.Map<CharacterDtoV2>(character) : null;
    }

    public async Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbClassJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        dbClassJobs = jobsService.GetCharacterClassJobs(lodestoneClassJobs.ClassJobDict, dbClassJobs).ToList();

        if (character is not null)
        {
            foreach (var dbClassJob in dbClassJobs.Where(x => x.CharacterId is null))
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
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.CharacterClassJobs)
            .FirstOrDefaultAsync();

        if (character is null)
        {
            return (new List<CharacterClassJobDto>(), null);
        }

        var classJobs = character.CharacterClassJobs.Select(mapper.Map<CharacterClassJobDto>);
        return (classJobs.ToList(), character.CharacterClassJobsUpdatedAt);
    }

    public async Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var classJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync();

        var classJobDtos = classJobs.Select(mapper.Map<CharacterClassJobDto>);
        return (classJobDtos.ToList(), classJobs.FirstOrDefault()?.Character?.CharacterClassJobsUpdatedAt);
    }

    public async Task<ICollection<CharacterMinionDto>> CacheCharacterMinionsAsync(string lodestoneId,
        CharacterCollectable lodestoneMinions)
    {
        using var activity = ActivitySource.StartActivity();

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
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Minions)
            .FirstOrDefaultAsync();

        if (character is null)
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
            .Include(x => x.Character)
            .ToListAsync();

        var minionDtos = minions.Select(mapper.Map<CharacterMinionDto>);
        return (minionDtos.ToList(), minions.FirstOrDefault()?.Character?.CharacterMinionsUpdatedAt);
    }

    public async Task<ICollection<CharacterMountDto>> CacheCharacterMountsAsync(string lodestoneId,
        CharacterCollectable lodestoneMounts)
    {
        using var activity = ActivitySource.StartActivity();

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
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Mounts)
            .FirstOrDefaultAsync();

        if (character is null)
        {
            return (new List<CharacterMountDto>(), null);
        }

        var mounts = character.Mounts.Select(mapper.Map<CharacterMountDto>);
        return (mounts.ToList(), character.CharacterMountsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(
        string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var mounts = await context.CharacterMounts.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync();

        var mountDtos = mounts.Select(mapper.Map<CharacterMountDto>);
        return (mountDtos.ToList(), mounts.FirstOrDefault()?.Character?.CharacterMountsUpdatedAt);
    }

    public async Task<ICollection<CharacterAchievementDto>> CacheCharacterAchievementsAsync(string lodestoneId,
        IEnumerable<CharacterAchievementEntry> lodestoneAchievements)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbAchievements = await context.CharacterAchievements.Where(x => x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        var newLodestoneAchievements = lodestoneAchievements.Where(x =>
            x.Id is not null && !dbAchievements.Select(y => y.AchievementId).Contains((ulong)x.Id!));
        var newDbAchievements = new List<CharacterAchievement>();
        foreach (var newLodestoneAchievement in newLodestoneAchievements)
        {
            var newDbAchievement = mapper.Map<CharacterAchievement>(newLodestoneAchievement);
            newDbAchievement.CharacterLodestoneId = lodestoneId;
            newDbAchievement.CharacterId = character?.Id ?? null;
            newDbAchievements.AddIfNotNull(newDbAchievement);
        }

        await context.CharacterAchievements.AddRangeAsync(newDbAchievements);
        dbAchievements.AddRange(newDbAchievements);

        // set FK if necessary
        if (character is not null)
        {
            var dbAchievementsWithoutFk = dbAchievements.Where(x => x.CharacterId is null);
            foreach (var dbMountWithoutFk in dbAchievementsWithoutFk)
            {
                dbMountWithoutFk.CharacterId = character.Id;
            }
        }

        if (character is not null)
        {
            character.CharacterAchievementsUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return dbAchievements.Select(mapper.Map<CharacterAchievementDto>).ToList();
    }

    public async Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)>
        GetCharacterAchievementsAsync(int id)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Achievements)
            .FirstOrDefaultAsync();

        if (character is null)
        {
            return (new List<CharacterAchievementDto>(), null);
        }

        var achievements = character.Achievements.Select(mapper.Map<CharacterAchievementDto>);
        return (achievements.ToList(), character.CharacterAchievementsUpdatedAt);
    }

    public async Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(
        string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var achievements = await context.CharacterAchievements.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync();

        var achievementDtos = achievements.Select(mapper.Map<CharacterAchievementDto>);
        return (achievementDtos.ToList(), achievements.FirstOrDefault()?.Character?.CharacterAchievementsUpdatedAt);
    }
}