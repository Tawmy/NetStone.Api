using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Extensions;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.Character;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.Achievement;
using NetStone.Model.Parseables.Character.Collectable;
using CharacterClassJob = NetStone.Model.Parseables.Character.ClassJob.CharacterClassJob;

namespace NetStone.Cache.Services;

internal class CharacterCachingServiceV3(
    DatabaseContext context)
    : ICharacterCachingServiceV3
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterCachingServiceV3));

    public async Task<CharacterDtoV3> CacheCharacterAsync(string lodestoneId, LodestoneCharacter lodestoneCharacter)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters
            .IncludeBasic()
            .Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);

        await using var transaction = await context.Database.BeginTransactionAsync();

        if (character is not null)
        {
            lodestoneCharacter.ToDb(character);
            character.Gear = CharacterGearServiceV3.GetGear(lodestoneCharacter.Gear, character.Gear);

            if (character.FreeCompany is not null &&
                character.FullFreeCompany?.LodestoneId != character.FreeCompany?.LodestoneId)
            {
                // free company has changed, try to match to possibly existing full fc profile
                var freeCompanyId = await context.FreeCompanies.Where(x =>
                        x.LodestoneId == character.FreeCompany!.LodestoneId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (freeCompanyId is not 0)
                {
                    character.FullFreeCompanyId = freeCompanyId;
                }
            }

            context.Entry(character).State = EntityState.Modified;
        }
        else
        {
            character = lodestoneCharacter.ToDb(lodestoneId);
            character.Gear = CharacterGearServiceV3.GetGear(lodestoneCharacter.Gear, []);

            // rely on EF to set FK for free company
            character.FreeCompany = lodestoneCharacter.FreeCompany?.ToDb();

            if (lodestoneCharacter.FreeCompany?.Id is { } freeCompanyLodestoneId)
            {
                // attach full free company to character if it was previously retrieved

                var freeCompanyId = await context.FreeCompanies.Where(x =>
                        x.LodestoneId == freeCompanyLodestoneId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (freeCompanyId is not 0)
                {
                    character.FullFreeCompanyId = freeCompanyId;
                }
            }

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
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }

        return character.ToDto();
    }

    public async Task<CharacterDtoV3?> GetCharacterAsync(int id)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.Id == id);
        return character?.ToDto();
    }

    public async Task<CharacterDtoV3?> GetCharacterAsync(string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return character?.ToDto();
    }

    public async Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync();

        var dbClassJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync();

        dbClassJobs = CharacterClassJobsServiceV3.GetCharacterClassJobs(lodestoneClassJobs.ClassJobDict, dbClassJobs)
            .ToList();

        if (character is not null)
        {
            foreach (var dbClassJob in dbClassJobs.Where(x => x.CharacterId is null))
            {
                // Try this each time as class jobs for character may previously have been retrieved without the character existing in the database
                // If so, FK can be set later
                dbClassJob.CharacterId = character.Id;
            }
        }

        // add new entries to context
        foreach (var newDbClassJob in dbClassJobs.Where(x => x.Id == 0))
        {
            newDbClassJob.CharacterLodestoneId = lodestoneId;
            await context.AddAsync(newDbClassJob);
        }

        if (character is not null)
        {
            character.CharacterClassJobsUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return dbClassJobs.Select(x => x.ToDto()).ToList();
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

        var classJobs = character.CharacterClassJobs.Select(x => x.ToDto());
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

        var classJobDtos = classJobs.Select(x => x.ToDto());
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
        var newDbMinions = newLodestoneMinions.Select(x => x.ToDbMinion(lodestoneId)).ToList();

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

        return dbMinions.Select(x => x.ToDto()).ToList();
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

        var minions = character.Minions.Select(x => x.ToDto());
        return (minions.ToList(), character.CharacterMinionsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(
        string lodestoneId)
    {
        var minions = await context.CharacterMinions.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync();

        var minionDtos = minions.Select(x => x.ToDto());
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
        var newDbMounts = newLodestoneMounts.Select(x => x.ToDbMount(lodestoneId)).ToList();

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

        return dbMounts.Select(x => x.ToDto()).ToList();
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

        var mounts = character.Mounts.Select(x => x.ToDto());
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

        var mountDtos = mounts.Select(x => x.ToDto());
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
        var newDbAchievements = newLodestoneAchievements.Select(x => x.ToDb(lodestoneId)).ToList();

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

        return dbAchievements.Select(x => x.ToDto()).ToList();
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

        var achievements = character.Achievements.Select(x => x.ToDto());
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

        var achievementDtos = achievements.Select(x => x.ToDto());
        return (achievementDtos.ToList(), achievements.FirstOrDefault()?.Character?.CharacterAchievementsUpdatedAt);
    }
}