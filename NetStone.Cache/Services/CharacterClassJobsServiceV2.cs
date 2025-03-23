using NetStone.Cache.Interfaces;
using NetStone.Common.Extensions;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.StaticData;
using CharacterClassJob = NetStone.Cache.Db.Models.CharacterClassJob;

namespace NetStone.Cache.Services;

public class CharacterClassJobsServiceV2(IAutoMapperService mapper)
{
    public ICollection<CharacterClassJob> GetCharacterClassJobs(
        IReadOnlyDictionary<ClassJob, ClassJobEntry> lodestoneClassJobs,
        ICollection<CharacterClassJob> dbClassJobs)
    {
        foreach (var lodestoneClassJob in lodestoneClassJobs)
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

    private IEnumerable<CharacterClassJob?> MapToDbEntry(
        KeyValuePair<ClassJob, ClassJobEntry> lodestoneClassJob, ICollection<CharacterClassJob> dbClassJobs)
    {
        return lodestoneClassJob.Key switch
        {
            ClassJob.Carpenter or ClassJob.Blacksmith or ClassJob.Armorer
                or ClassJob.Goldsmith or ClassJob.Leatherworker or ClassJob.Weaver
                or ClassJob.Alchemist or ClassJob.Culinarian or ClassJob.Miner
                or ClassJob.Botanist or ClassJob.Fisher or ClassJob.Machinist
                or ClassJob.DarkKnight or ClassJob.Astrologian or ClassJob.Samurai
                or ClassJob.RedMage or ClassJob.Pictomancer or ClassJob.BlueMage or ClassJob.Gunbreaker
                or ClassJob.Dancer or ClassJob.Reaper or ClassJob.Viper or ClassJob.Sage
                or ClassJob.Gladiator or ClassJob.Pugilist or ClassJob.Marauder
                or ClassJob.Lancer or ClassJob.Archer or ClassJob.Conjurer
                or ClassJob.Thaumaturge or ClassJob.Arcanist
                or ClassJob.Rogue => [ToClassJob(lodestoneClassJob, dbClassJobs)],
            ClassJob.Paladin =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Gladiator)
            ],
            ClassJob.Monk =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Pugilist)
            ],
            ClassJob.Warrior =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Marauder)
            ],
            ClassJob.Dragoon =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Lancer)
            ],
            ClassJob.Bard =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Archer)
            ],
            ClassJob.WhiteMage =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Conjurer)
            ],
            ClassJob.BlackMage =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.BlackMage)
            ],
            ClassJob.Summoner =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Scholar),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Arcanist)
            ],
            ClassJob.Scholar =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Summoner),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Arcanist)
            ],
            ClassJob.Ninja =>
            [
                ToClassJob(lodestoneClassJob, dbClassJobs),
                ToClassJob(lodestoneClassJob, dbClassJobs, Common.Enums.ClassJob.Rogue)
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(lodestoneClassJob),
                $"ClassJob {lodestoneClassJob.Key} does not exist.")
        };
    }

    private CharacterClassJob? ToClassJob(KeyValuePair<ClassJob, ClassJobEntry> lodestoneClassJob,
        IEnumerable<CharacterClassJob> dbClassJobs, Common.Enums.ClassJob? classJobOverride = null)
    {
        if (!lodestoneClassJob.Value.IsUnlocked)
        {
            return null;
        }

        var classJob = mapper.Map<CharacterClassJob>(lodestoneClassJob.Value);
        classJob.ClassJob = classJobOverride ?? Enum.Parse<Common.Enums.ClassJob>(lodestoneClassJob.Key.ToString());

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