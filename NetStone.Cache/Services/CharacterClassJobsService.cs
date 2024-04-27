using AutoMapper;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character.ClassJob;
using CharacterClassJob = NetStone.Cache.Db.Models.CharacterClassJob;

namespace NetStone.Cache.Services;

public class CharacterClassJobsService(IMapper mapper)
{
    public ICollection<CharacterClassJob> GetCharacterClassJobs(
        Model.Parseables.Character.ClassJob.CharacterClassJob lodestoneClassJobs,
        ICollection<CharacterClassJob> dbClassJobs)
    {
        foreach (var lodestoneClassJob in lodestoneClassJobs.ClassJobDict)
        {
            // iterate all class jobs returned from Lodestone

            foreach (var newDbClassJob in MapToDbEntry(lodestoneClassJob, dbClassJobs))
            {
                // iterate all possible mappings returned from method
                // parser returns nonsense for some job mappings, so we need this workaround

                if (newDbClassJob is null)
                {
                    continue;
                }

                if (dbClassJobs.FirstOrDefault(x => x.ClassJob == newDbClassJob.ClassJob) is { } existingDbClassJob)
                {
                    mapper.Map(newDbClassJob, existingDbClassJob);
                }
                else
                {
                    dbClassJobs.Add(newDbClassJob);
                }
            }
        }

        return dbClassJobs;
    }

    private ClassJob ParseClassJob(KeyValuePair<StaticData.ClassJob, ClassJobEntry> classJobEntry)
    {
        // TODO remove!!!

        // try to parse name first since in some cases ClassJob is wrong!
        return Enum.TryParse<ClassJob>(classJobEntry.Value.Name, true, out var resultName)
            ? resultName
            : Enum.Parse<ClassJob>(classJobEntry.Key.ToString());
    }

    private IEnumerable<CharacterClassJob?> MapToDbEntry(
        KeyValuePair<StaticData.ClassJob, ClassJobEntry?> lodestoneClassJob, ICollection<CharacterClassJob> dbClassJobs)
    {
        return lodestoneClassJob.Key switch
        {
            StaticData.ClassJob.Carpenter or StaticData.ClassJob.Blacksmith or StaticData.ClassJob.Armorer
                or StaticData.ClassJob.Goldsmith or StaticData.ClassJob.Leatherworker or StaticData.ClassJob.Weaver
                or StaticData.ClassJob.Alchemist or StaticData.ClassJob.Culinarian or StaticData.ClassJob.Miner
                or StaticData.ClassJob.Botanist or StaticData.ClassJob.Fisher or StaticData.ClassJob.Machinist
                or StaticData.ClassJob.DarkKnight or StaticData.ClassJob.Astrologian or StaticData.ClassJob.Samurai
                or StaticData.ClassJob.RedMage or StaticData.ClassJob.BlueMage or StaticData.ClassJob.Gunbreaker
                or StaticData.ClassJob.Dancer or StaticData.ClassJob.Reaper or StaticData.ClassJob.Sage
                or StaticData.ClassJob.Gladiator or StaticData.ClassJob.Pugilist or StaticData.ClassJob.Marauder
                or StaticData.ClassJob.Lancer or StaticData.ClassJob.Archer or StaticData.ClassJob.Conjurer
                or StaticData.ClassJob.Thaumaturge or StaticData.ClassJob.Arcanist
                or StaticData.ClassJob.Rogue => [ToClassJob(lodestoneClassJob, dbClassJobs)],
            StaticData.ClassJob.Paladin =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Gladiator)
            ],
            StaticData.ClassJob.Monk =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Pugilist)
            ],
            StaticData.ClassJob.Warrior =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Marauder)
            ],
            StaticData.ClassJob.Dragoon =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Lancer)
            ],
            StaticData.ClassJob.Bard =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Archer)
            ],
            StaticData.ClassJob.WhiteMage =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Conjurer)
            ],
            StaticData.ClassJob.BlackMage =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.BlackMage)
            ],
            StaticData.ClassJob.Summoner =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Scholar),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Arcanist)
            ],
            StaticData.ClassJob.Scholar =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Summoner),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Arcanist)
            ],
            StaticData.ClassJob.Ninja =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, ClassJob.Rogue)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(lodestoneClassJob),
                $"ClassJob {lodestoneClassJob.Key} does not exist.")
        };
    }

    private CharacterClassJob? ToClassJob(KeyValuePair<StaticData.ClassJob, ClassJobEntry?> lodestoneClassJob,
        IEnumerable<CharacterClassJob> dbClassJobs, ClassJob? classJobOverride = null)
    {
        if (lodestoneClassJob.Value is null)
        {
            return null;
        }

        var classJob = mapper.Map<CharacterClassJob>(lodestoneClassJob.Value);
        classJob.ClassJob = classJobOverride ?? ParseClassJob(lodestoneClassJob!);

        if (classJob.ClassJob.EvolvesFromClass() && !classJob.IsJobUnlocked)
        {
            // do not add job to database if not actually unlocked. NetStone returns nonsense.
            return null;
        }

        var currentDbClassJob = dbClassJobs.FirstOrDefault(x => x.ClassJob == classJob.ClassJob);

        classJob.Id = currentDbClassJob?.Id ?? default;
        classJob.CharacterId = currentDbClassJob?.CharacterId ?? null;
        classJob.CharacterLodestoneId = currentDbClassJob?.CharacterLodestoneId ?? string.Empty;

        return classJob;
    }
}