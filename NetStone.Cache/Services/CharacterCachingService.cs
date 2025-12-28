using System.Diagnostics;
using AspNetCoreExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

internal class CharacterCachingService(DatabaseContext context, IS3Service s3, IConfiguration config)
    : ICharacterCachingService
{
    private static readonly ActivitySource ActivitySource = new(nameof(ICharacterCachingService));

    public async Task<CharacterDto> CacheCharacterAsync(string lodestoneId, LodestoneCharacter lodestoneCharacter,
        bool cacheImages, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters
            .IncludeBasic()
            .Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId, ct);

        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        string? avatarS3 = null;
        string? portraitS3 = null;
        if (cacheImages)
        {
            var bucket = config.GetGuardedConfiguration(EnvironmentVariables.S3BucketName);
            avatarS3 = await s3.ReuploadAsync(bucket, $"avatar_{lodestoneId}_{Guid.CreateVersion7()}.jpg",
                lodestoneCharacter.Avatar ??
                throw new ArgumentNullException(nameof(lodestoneCharacter), nameof(lodestoneCharacter.Avatar)),
                character?.AvatarS3, ct);

            portraitS3 = await s3.ReuploadAsync(bucket, $"portrait_{lodestoneId}_{Guid.CreateVersion7()}.jpg",
                lodestoneCharacter.Portrait ??
                throw new ArgumentNullException(nameof(lodestoneCharacter), nameof(lodestoneCharacter.Portrait)),
                character?.PortraitS3, ct);
        }

        if (character is not null)
        {
            lodestoneCharacter.ToDb(character, avatarS3, portraitS3);
            character.Gear = CharacterGearService.GetGear(lodestoneCharacter.Gear, character.Gear);

            if (character.FreeCompany is not null)
            {
                if (character.FullFreeCompany?.LodestoneId != character.FreeCompany?.LodestoneId)
                {
                    // free company has changed, try to match to possibly existing full fc profile
                    var freeCompanyId = await context.FreeCompanies.Where(x =>
                            x.LodestoneId == character.FreeCompany!.LodestoneId)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync(ct);

                    if (freeCompanyId is not 0)
                    {
                        // character switched to different fc and it exists in the database, attach it
                        character.FullFreeCompanyId = freeCompanyId;
                    }
                    else
                    {
                        // character switched to different fc and it exists in the database, remove existing
                        character.FullFreeCompanyId = null;
                    }
                }
            }
            else if (character.FullFreeCompanyId is not null)
            {
                // character not in fc, set full free company id to cover case of user leaving 
                character.FullFreeCompanyId = null;
            }

            context.Entry(character).State = EntityState.Modified;
        }
        else
        {
            character = lodestoneCharacter.ToDb(lodestoneId, avatarS3, portraitS3);
            character.Gear = CharacterGearService.GetGear(lodestoneCharacter.Gear, []);

            // rely on EF to set FK for free company
            character.FreeCompany = lodestoneCharacter.FreeCompany?.ToDb();

            if (lodestoneCharacter.FreeCompany?.Id is { } freeCompanyLodestoneId)
            {
                // attach full free company to character if it was previously retrieved

                var freeCompanyId = await context.FreeCompanies.Where(x =>
                        x.LodestoneId == freeCompanyLodestoneId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(ct);

                if (freeCompanyId is not 0)
                {
                    character.FullFreeCompanyId = freeCompanyId;
                }
            }

            await context.Characters.AddAsync(character, ct);
            await context.SaveChangesAsync(ct);

            var classJobs = await context.CharacterClassJobs.Where(x =>
                    x.CharacterLodestoneId == character.LodestoneId)
                .ToListAsync(ct);
            classJobs.ForEach(x => x.CharacterId = character.Id);

            var minions = await context.CharacterMinions.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync(ct);
            minions.ForEach(x => x.CharacterId = character.Id);

            var mounts = await context.CharacterMounts.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync(ct);
            mounts.ForEach(x => x.CharacterId = character.Id);

            var achievements = await context.CharacterAchievements.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync(ct);
            achievements.ForEach(x => x.CharacterId = character.Id);

            var freeCompanyMembers = await context.FreeCompanyMembers.Where(x =>
                    x.CharacterLodestoneId == lodestoneId)
                .ToListAsync(ct);
            freeCompanyMembers.ForEach(x => x.FullCharacterId = character.Id);
        }

        character.CharacterUpdatedAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }

        return character.ToDto();
    }

    public async Task<CharacterDto?> GetCharacterAsync(int id, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.Id == id, ct);
        return character?.ToDto();
    }

    public async Task<CharacterDto?> GetCharacterAsync(string lodestoneId, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId, ct);
        return character?.ToDto();
    }

    public async Task<CharacterDto?> GetCharacterAsync(string name, string world, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.IncludeBasic().Include(x => x.FullFreeCompany)
            .Where(x => // case insensitive search with ILIKE
                EF.Functions.ILike(x.Name, name) &&
                EF.Functions.ILike(x.Server, world))
            .SingleOrDefaultAsync(ct);

        return character?.ToDto();
    }

    public async Task<ICollection<CharacterClassJobDto>> CacheCharacterClassJobsAsync(string lodestoneId,
        CharacterClassJob lodestoneClassJobs, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync(ct);

        var dbClassJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .ToListAsync(ct);

        dbClassJobs = CharacterClassJobsService.GetCharacterClassJobs(lodestoneClassJobs.ClassJobDict, dbClassJobs)
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
            await context.AddAsync(newDbClassJob, ct);
        }

        if (character is not null)
        {
            character.CharacterClassJobsUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(ct);

        return dbClassJobs.Select(x => x.ToDto()).ToList();
    }

    public async Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)>
        GetCharacterClassJobsAsync(int id, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.CharacterClassJobs)
            .FirstOrDefaultAsync(ct);

        if (character is null)
        {
            return (new List<CharacterClassJobDto>(), null);
        }

        var classJobs = character.CharacterClassJobs.Select(x => x.ToDto());
        return (classJobs.ToList(), character.CharacterClassJobsUpdatedAt);
    }

    public async Task<(ICollection<CharacterClassJobDto> classJobs, DateTime? LastUpdated)> GetCharacterClassJobsAsync(
        string lodestoneId, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var classJobs = await context.CharacterClassJobs.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync(ct);

        var classJobDtos = classJobs.Select(x => x.ToDto());
        return (classJobDtos.ToList(), classJobs.FirstOrDefault()?.Character?.CharacterClassJobsUpdatedAt);
    }

    public async Task<ICollection<CharacterMinionDto>> CacheCharacterMinionsAsync(string lodestoneId,
        CharacterCollectable lodestoneMinions, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync(ct);

        var dbMinions = await context.CharacterMinions.Where(x => x.CharacterLodestoneId == lodestoneId)
            .ToListAsync(ct);

        // add new Lodestone minions
        var newLodestoneMinions =
            lodestoneMinions.Collectables.Where(x => !dbMinions.Select(y => y.Name).Contains(x.Name));
        var newDbMinions = newLodestoneMinions.Select(x => x.ToDbMinion(lodestoneId)).ToList();

        await context.CharacterMinions.AddRangeAsync(newDbMinions, ct);
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

        await context.SaveChangesAsync(ct);

        return dbMinions.Select(x => x.ToDto()).ToList();
    }

    public async Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(int id,
        CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Minions)
            .FirstOrDefaultAsync(ct);

        if (character is null)
        {
            return (new List<CharacterMinionDto>(), null);
        }

        var minions = character.Minions.Select(x => x.ToDto());
        return (minions.ToList(), character.CharacterMinionsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMinionDto>, DateTime? LastUpdated)> GetCharacterMinionsAsync(
        string lodestoneId, CT ct = default)
    {
        var minions = await context.CharacterMinions.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync(ct);

        var minionDtos = minions.Select(x => x.ToDto());
        return (minionDtos.ToList(), minions.FirstOrDefault()?.Character?.CharacterMinionsUpdatedAt);
    }

    public async Task<ICollection<CharacterMountDto>> CacheCharacterMountsAsync(string lodestoneId,
        CharacterCollectable lodestoneMounts, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync(ct);

        var dbMounts = await context.CharacterMounts.Where(x => x.CharacterLodestoneId == lodestoneId).ToListAsync(ct);

        // add new Lodestone mounts
        var newLodestoneMounts =
            lodestoneMounts.Collectables.Where(x => !dbMounts.Select(y => y.Name).Contains(x.Name));
        var newDbMounts = newLodestoneMounts.Select(x => x.ToDbMount(lodestoneId)).ToList();

        await context.CharacterMounts.AddRangeAsync(newDbMounts, ct);
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

        await context.SaveChangesAsync(ct);

        return dbMounts.Select(x => x.ToDto()).ToList();
    }

    public async Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(int id,
        CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Mounts)
            .FirstOrDefaultAsync(ct);

        if (character is null)
        {
            return (new List<CharacterMountDto>(), null);
        }

        var mounts = character.Mounts.Select(x => x.ToDto());
        return (mounts.ToList(), character.CharacterMountsUpdatedAt);
    }

    public async Task<(ICollection<CharacterMountDto>, DateTime? LastUpdated)> GetCharacterMountsAsync(
        string lodestoneId, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var mounts = await context.CharacterMounts.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync(ct);

        var mountDtos = mounts.Select(x => x.ToDto());
        return (mountDtos.ToList(), mounts.FirstOrDefault()?.Character?.CharacterMountsUpdatedAt);
    }

    public async Task<ICollection<CharacterAchievementDto>> CacheCharacterAchievementsAsync(string lodestoneId,
        IEnumerable<CharacterAchievementEntry> lodestoneAchievements, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x => x.LodestoneId == lodestoneId).FirstOrDefaultAsync(ct);

        var dbAchievements = await context.CharacterAchievements.Where(x => x.CharacterLodestoneId == lodestoneId)
            .ToListAsync(ct);

        var newLodestoneAchievements = lodestoneAchievements.Where(x =>
            x.Id is not null && !dbAchievements.Select(y => y.AchievementId).Contains((ulong)x.Id!));
        var newDbAchievements = newLodestoneAchievements.Select(x => x.ToDb(lodestoneId)).ToList();

        await context.CharacterAchievements.AddRangeAsync(newDbAchievements, ct);
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

        await context.SaveChangesAsync(ct);

        return dbAchievements.Select(x => x.ToDto()).ToList();
    }

    public async Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)>
        GetCharacterAchievementsAsync(int id, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var character = await context.Characters.Where(x =>
                x.Id == id)
            .Include(x =>
                x.Achievements)
            .FirstOrDefaultAsync(ct);

        if (character is null)
        {
            return (new List<CharacterAchievementDto>(), null);
        }

        var achievements = character.Achievements.Select(x => x.ToDto());
        return (achievements.ToList(), character.CharacterAchievementsUpdatedAt);
    }

    public async Task<(ICollection<CharacterAchievementDto>, DateTime? LastUpdated)> GetCharacterAchievementsAsync(
        string lodestoneId, CT ct = default)
    {
        using var activity = ActivitySource.StartActivity();

        var achievements = await context.CharacterAchievements.Where(x =>
                x.CharacterLodestoneId == lodestoneId)
            .Include(x => x.Character)
            .ToListAsync(ct);

        var achievementDtos = achievements.Select(x => x.ToDto());
        return (achievementDtos.ToList(), achievements.FirstOrDefault()?.Character?.CharacterAchievementsUpdatedAt);
    }
}