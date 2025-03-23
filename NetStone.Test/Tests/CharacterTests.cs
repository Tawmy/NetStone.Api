using NetStone.Cache.Extensions;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;
using NetStone.Common.Helpers;
using NetStone.Model.Parseables.Character;
using NetStone.Model.Parseables.Character.ClassJob;
using NetStone.Model.Parseables.Character.Gear;
using NetStone.Test.DataGenerators;
using NetStone.Test.Fixtures;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using CharacterAttributes = NetStone.Model.Parseables.Character.CharacterAttributes;
using CharacterGear = NetStone.Model.Parseables.Character.Gear.CharacterGear;
using ClassJob = NetStone.StaticData.ClassJob;

namespace NetStone.Test.Tests;

public class CharacterTests(ITestOutputHelper testOutputHelper, CharacterTestsFixture fixture)
    : TestBed<CharacterTestsFixture>(testOutputHelper, fixture)
{
    private readonly INetStoneService _netStoneService = fixture.GetService<INetStoneService>(testOutputHelper)!;

    #region CharacterClassJobs

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterClassJobsMatchDto(string lodestoneId)
    {
        var classJobsLodestone = await _netStoneService.GetCharacterClassJob(lodestoneId);
        Assert.NotNull(classJobsLodestone);

        foreach (var (key, classJobLodestone) in classJobsLodestone.ClassJobDict.Where(x => x.Value.IsUnlocked))
        {
            var classJobDb = CharacterClassJobsServiceV3
                .GetCharacterClassJobs(new Dictionary<ClassJob, ClassJobEntry> { { key, classJobLodestone } }, [])
                .FirstOrDefault();

            if (classJobDb is null)
            {
                continue;
                // TODO base classes are filtered out once job unlocked. Potentially add integration test for this later.
                // actually, most definitely add test for this. it seems to falsely return conjurer for Sigyn.
            }

            var classJobDto = classJobDb.ToDto();
            Assert.NotNull(classJobDto);

            Assert.Equal(classJobLodestone.IsJobUnlocked, classJobDto.IsJobUnlocked);
            Assert.Equal(classJobLodestone.Level, classJobDto.Level);
            Assert.Equal(classJobLodestone.ExpCurrent, classJobDto.ExpCurrent);
            Assert.Equal(classJobLodestone.ExpMax, classJobDto.ExpMax);
            Assert.Equal(classJobLodestone.ExpToGo, classJobDto.ExpToGo);
            Assert.Equal(classJobLodestone.IsSpecialized, classJobDto.IsSpecialized);
        }
    }

    #endregion

    #region CharacterMinions

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterMinionsMatchDto(string lodestoneId)
    {
        var minionsLodestone = await _netStoneService.GetCharacterMinion(lodestoneId);

        if (lodestoneId == "45386124") // Testerinus Maximus, Phoenix)
        {
            // test character has no minions
            Assert.Null(minionsLodestone);
            return;
        }

        Assert.NotNull(minionsLodestone);

        foreach (var minionLodestone in minionsLodestone.Collectables)
        {
            var minionDb = minionLodestone.ToDbMinion(lodestoneId);
            Assert.NotNull(minionDb);

            var minionDto = minionDb.ToDto();
            Assert.NotNull(minionDb);

            Assert.Equal(minionLodestone.Name, minionDto.Name);
        }
    }

    #endregion

    #region CharacterMounts

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterMountsMatchDto(string lodestoneId)
    {
        var mountsLodestone = await _netStoneService.GetCharacterMount(lodestoneId);

        if (new[] { "45386124", "28835226" }
            .Contains(lodestoneId)) // Testerinus Maximus, Phoenix; Hena Wilbert, Phoenix)
        {
            // test character has no mounts
            Assert.Null(mountsLodestone);
            return;
        }

        Assert.NotNull(mountsLodestone);

        foreach (var mountLodestone in mountsLodestone.Collectables)
        {
            var mountDb = mountLodestone.ToDbMount(lodestoneId);
            Assert.NotNull(mountDb);

            var mountDto = mountDb.ToDto();
            Assert.NotNull(mountDb);

            Assert.Equal(mountLodestone.Name, mountDto.Name);
        }
    }

    #endregion

    #region CharacterAchievements

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterAchievementsMatchDto(string lodestoneId)
    {
        var achievementsLodestone = await _netStoneService.GetCharacterAchievement(lodestoneId);
        Assert.NotNull(achievementsLodestone);

        foreach (var achievementLodestone in achievementsLodestone.Achievements)
        {
            var achievementDb = achievementLodestone.ToDb(lodestoneId);
            Assert.NotNull(achievementDb);

            var achievementDto = achievementDb.ToDto();
            Assert.NotNull(achievementDto);

            Assert.Equal(achievementLodestone.Id, achievementDto.Id);
            Assert.Equal(achievementLodestone.Name, achievementDto.Name);
            Assert.Equal(achievementLodestone.DatabaseLink, achievementDto.DatabaseLink);
            Assert.Equal(achievementLodestone.TimeAchieved, achievementDto.TimeAchieved);
        }
    }

    #endregion

    #region CharacterSearch

    [Theory]
    [ClassData(typeof(CharacterSearchDataGenerator))]
    public async Task ApiCharacterSearch(CharacterSearchTestData data)
    {
        var netStoneQuery = data.Query.ToNetStone();
        var searchResult = await _netStoneService.SearchCharacter(netStoneQuery, data.Page ?? 1);
        Assert.NotNull(searchResult);

        if (data.ExpectedResults is -1)
        {
            Assert.True(searchResult.Results.Count() > 1, "Expected more than one result.");
        }
        else
        {
            Assert.Equal(data.ExpectedResults, searchResult.Results.Count());
        }
    }

    #endregion

    #region Character

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterMatchesDto(string lodestoneId)
    {
        var characterLodestone = await _netStoneService.GetCharacter(lodestoneId);
        Assert.NotNull(characterLodestone);

        var characterDb = characterLodestone.ToDb(lodestoneId);
        characterDb.Gear = CharacterGearServiceV3.GetGear(characterLodestone.Gear, characterDb.Gear);
        Assert.NotNull(characterDb);

        characterDb.LodestoneId = lodestoneId; // also set manually in code

        var characterDto = characterDb.ToDto();
        Assert.NotNull(characterDto);

        Assert.Equal(lodestoneId, characterDto.Id);
        Assert.Equal(characterLodestone.Name, characterDto.Name);
        Assert.Equal(characterLodestone.Server, characterDto.Server);
        Assert.Equal(characterLodestone.Title, characterDto.Title);
        Assert.Equal(characterLodestone.Avatar?.ToString(), characterDto.Avatar);
        Assert.Equal(characterLodestone.Portrait?.ToString(), characterDto.Portrait);
        Assert.Equal(characterLodestone.Bio, characterDto.Bio);
        Assert.Equal(characterLodestone.Nameday, characterDto.Nameday);

        Assert.Equal(characterLodestone.GetActiveClassJob(), characterDto.ActiveClassJob);
        Assert.Equal(characterLodestone.ActiveClassJobLevel, characterDto.ActiveClassJobLevel);
        Assert.Equal(characterLodestone.ActiveClassJobIcon, characterDto.ActiveClassJobIcon);

        if (!string.IsNullOrWhiteSpace(characterLodestone.GrandCompanyName))
        {
            Assert.NotEqual(GrandCompany.NoAffiliation, characterDto.GrandCompany);
        }

        Assert.Equal(characterLodestone.GrandCompanyRank, characterDto.GrandCompanyRank);

        if (characterLodestone.FreeCompany is null)
        {
            Assert.Null(characterDto.FreeCompany);
        }
        else
        {
            Assert.NotNull(characterDto.FreeCompany);

            Assert.Equal(characterLodestone.FreeCompany.Id, characterDto.FreeCompany.Id);
            Assert.Equal(characterLodestone.FreeCompany.Name, characterDto.FreeCompany.Name);
            Assert.Equal(characterLodestone.FreeCompany.Link?.ToString(), characterDto.FreeCompany.Link);

            Assert.Equal(characterLodestone.FreeCompany.IconLayers.TopLayer?.ToString(),
                characterDto.FreeCompany.IconLayers.TopLayer);
            Assert.Equal(characterLodestone.FreeCompany.IconLayers.MiddleLayer?.ToString(),
                characterDto.FreeCompany.IconLayers.MiddleLayer);
            Assert.Equal(characterLodestone.FreeCompany.IconLayers.BottomLayer?.ToString(),
                characterDto.FreeCompany.IconLayers.BottomLayer);
        }

        Assert.Equal(characterLodestone.GuardianDeityName, characterDto.GuardianDeityName);
        Assert.Equal(characterLodestone.GuardianDeityIcon?.ToString(), characterDto.GuardianDeityIcon);

        Assert.Equal(characterLodestone.PvPTeam?.Name, characterDto.PvpTeam);
        Assert.Equal(EnumHelper.ParseFromDisplayString<Race>(characterLodestone.Race), characterDto.Race);
        Assert.Equal(EnumHelper.ParseFromDisplayString<Tribe>(characterLodestone.Tribe), characterDto.Tribe);

        switch (characterLodestone.Gender)
        {
            case LodestoneCharacter.MaleChar:
                Assert.Equal(Gender.Male, characterDto.Gender);
                break;
            case LodestoneCharacter.FemaleChar:
                Assert.Equal(Gender.Female, characterDto.Gender);
                break;
        }

        Assert.Equal(characterLodestone.TownName, characterDto.TownName);
        Assert.Equal(characterLodestone.TownIcon?.ToString(), characterDto.TownIcon);

        CompareGear(characterLodestone.Gear, characterDto);
        CompareAttributes(characterLodestone.Attributes, characterDto);
    }

    private static void CompareGear(CharacterGear gearLodestone, CharacterDtoV3 characterDto)
    {
        var gearLodestoneDict = new Dictionary<GearSlot, GearEntry>();
        gearLodestoneDict.AddIfValueNotNull(GearSlot.MainHand, gearLodestone.Mainhand);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.OffHand, gearLodestone.Offhand);

        gearLodestoneDict.AddIfValueNotNull(GearSlot.Head, gearLodestone.Head);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Body, gearLodestone.Body);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Hands, gearLodestone.Hands);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Legs, gearLodestone.Legs);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Feet, gearLodestone.Feet);

        gearLodestoneDict.AddIfValueNotNull(GearSlot.Earrings, gearLodestone.Earrings);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Necklace, gearLodestone.Necklace);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Bracelets, gearLodestone.Bracelets);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Ring1, gearLodestone.Ring1);
        gearLodestoneDict.AddIfValueNotNull(GearSlot.Ring2, gearLodestone.Ring2);

        foreach (var (slot, gearPieceLodestone) in gearLodestoneDict)
        {
            var gearPieceDto = characterDto.Gear.FirstOrDefault(x => x.Slot == slot);
            Assert.NotNull(gearPieceDto);

            Assert.Equal(gearPieceLodestone.ItemName, gearPieceDto.ItemName);
            Assert.Equal(gearPieceLodestone.ItemLevel, gearPieceDto.ItemLevel);
            Assert.Equal(gearPieceLodestone.ItemDatabaseLink?.ToString(), gearPieceDto.ItemDatabaseLink);
            Assert.Equal(gearPieceLodestone.IsHq, gearPieceDto.IsHq);
            Assert.Equal(gearPieceLodestone.StrippedItemName, gearPieceDto.StrippedItemName);
            Assert.Equal(gearPieceLodestone.GlamourDatabaseLink?.ToString(), gearPieceDto.GlamourDatabaseLink);
            Assert.Equal(gearPieceLodestone.GlamourName, gearPieceDto.GlamourName);
            Assert.Equal(gearPieceLodestone.CreatorName, gearPieceDto.CreatorName);

            foreach (var materiaLodestone in gearPieceLodestone.Materia)
            {
                Assert.Contains(materiaLodestone, gearPieceDto.Materia);
            }
        }
    }

    private static void CompareAttributes(CharacterAttributes attributes, CharacterDtoV3 characterDto)
    {
        Assert.Equal(attributes.Strength, characterDto.Attributes[CharacterAttribute.Strength]);
        Assert.Equal(attributes.Dexterity, characterDto.Attributes[CharacterAttribute.Dexterity]);
        Assert.Equal(attributes.Vitality, characterDto.Attributes[CharacterAttribute.Vitality]);
        Assert.Equal(attributes.Intelligence, characterDto.Attributes[CharacterAttribute.Intelligence]);
        Assert.Equal(attributes.Mind, characterDto.Attributes[CharacterAttribute.Mind]);
        Assert.Equal(attributes.CriticalHitRate, characterDto.Attributes[CharacterAttribute.CriticalHitRate]);
        Assert.Equal(attributes.Determination, characterDto.Attributes[CharacterAttribute.Determination]);
        Assert.Equal(attributes.DirectHitRate, characterDto.Attributes[CharacterAttribute.DirectHitRate]);
        Assert.Equal(attributes.Defense, characterDto.Attributes[CharacterAttribute.Defense]);
        Assert.Equal(attributes.MagicDefense, characterDto.Attributes[CharacterAttribute.MagicDefense]);
        Assert.Equal(attributes.AttackPower, characterDto.Attributes[CharacterAttribute.AttackPower]);
        Assert.Equal(attributes.SkillSpeed, characterDto.Attributes[CharacterAttribute.SkillSpeed]);

        characterDto.Attributes.TryGetValue(CharacterAttribute.AttackMagicPotency, out var attackMagicPotency);
        Assert.Equal(attributes.AttackMagicPotency, attackMagicPotency);

        characterDto.Attributes.TryGetValue(CharacterAttribute.HealingMagicPotency, out var healingMagicPotency);
        Assert.Equal(attributes.HealingMagicPotency, healingMagicPotency);

        characterDto.Attributes.TryGetValue(CharacterAttribute.SpellSpeed, out var spellSpeed);
        Assert.Equal(attributes.SpellSpeed, spellSpeed);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Craftsmanship, out var craftsmanship);
        Assert.Equal(attributes.Craftsmanship, craftsmanship);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Control, out var control);
        Assert.Equal(attributes.Control, control);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Gathering, out var gathering);
        Assert.Equal(attributes.Gathering, gathering);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Perception, out var perception);
        Assert.Equal(attributes.Perception, perception);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Tenacity, out var tenacity);
        Assert.Equal(attributes.Tenacity, tenacity);

        characterDto.Attributes.TryGetValue(CharacterAttribute.Piety, out var piety);
        Assert.Equal(attributes.Piety, piety);

        if (characterDto.ActiveClassJob.IsDiscipleOfHand())
        {
            Assert.Equal(attributes.MpGpCp, characterDto.Attributes[CharacterAttribute.Cp]);
        }
        else if (characterDto.ActiveClassJob.IsDiscipleOfLand())
        {
            Assert.Equal(attributes.MpGpCp, characterDto.Attributes[CharacterAttribute.Gp]);
        }
        else
        {
            Assert.Equal(attributes.MpGpCp, characterDto.Attributes[CharacterAttribute.Mp]);
        }
    }

    #endregion
}