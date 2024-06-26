using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions;
using NetStone.Cache.Services;
using NetStone.Common.DTOs.Character;
using NetStone.Common.Enums;
using NetStone.Common.Extensions;
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
    private readonly LodestoneClient _client = fixture.GetService<LodestoneClient>(testOutputHelper)!;

    private readonly CharacterClassJobsService _jobService =
        fixture.GetService<CharacterClassJobsService>(testOutputHelper)!;

    private readonly IMapper _mapper = fixture.GetService<IMapper>(testOutputHelper)!;

    #region CharacterClassJobs

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterClassJobsMatchDto(string lodestoneId)
    {
        var classJobsLodestone = await _client.GetCharacterClassJob(lodestoneId);
        Assert.NotNull(classJobsLodestone);

        foreach (var (key, classJobLodestone) in classJobsLodestone.ClassJobDict.Where(x => x.Value is not null))
        {
            var classJobDb = _jobService
                .GetCharacterClassJobs(new Dictionary<ClassJob, ClassJobEntry?> { { key, classJobLodestone } }, [])
                .FirstOrDefault();

            if (classJobDb is null)
            {
                continue;
                // TODO base classes are filtered out once job unlocked. Potentially add integration test for this later.
                // actually, most definitely add test for this. it seems to falsely return conjurer for Sigyn.
            }

            var classJobDto = _mapper.Map<CharacterClassJobDto>(classJobDb);
            Assert.NotNull(classJobDto);

            Assert.Equal(classJobLodestone?.IsJobUnlocked, classJobDto.IsJobUnlocked);
            Assert.Equal(classJobLodestone?.Level, classJobDto.Level);
            Assert.Equal(classJobLodestone?.ExpCurrent, classJobDto.ExpCurrent);
            Assert.Equal(classJobLodestone?.ExpMax, classJobDto.ExpMax);
            Assert.Equal(classJobLodestone?.ExpToGo, classJobDto.ExpToGo);
            Assert.Equal(classJobLodestone?.IsSpecialized, classJobDto.IsSpecialized);
        }
    }

    #endregion

    #region CharacterMinions

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterMinionsMatchDto(string lodestoneId)
    {
        var minionsLodestone = await _client.GetCharacterMinion(lodestoneId);
        Assert.NotNull(minionsLodestone);

        foreach (var minionLodestone in minionsLodestone.Collectables)
        {
            var minionDb = _mapper.Map<CharacterMinion>(minionLodestone);
            Assert.NotNull(minionDb);

            var minionDto = _mapper.Map<CharacterMinionDto>(minionDb);
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
        var mountsLodestone = await _client.GetCharacterMount(lodestoneId);
        Assert.NotNull(mountsLodestone);

        foreach (var mountLodestone in mountsLodestone.Collectables)
        {
            var mountDb = _mapper.Map<CharacterMinion>(mountLodestone);
            Assert.NotNull(mountDb);

            var mountDto = _mapper.Map<CharacterMinionDto>(mountDb);
            Assert.NotNull(mountDb);

            Assert.Equal(mountLodestone.Name, mountDto.Name);
        }
    }

    #endregion

    #region Character

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task ApiCharacterMatchesDto(string lodestoneId)
    {
        var characterLodestone = await _client.GetCharacter(lodestoneId);
        Assert.NotNull(characterLodestone);

        var characterDb = _mapper.Map<Character>(characterLodestone);
        Assert.NotNull(characterDb);

        characterDb.LodestoneId = lodestoneId; // also set manually in code

        var characterDto = _mapper.Map<CharacterDto>(characterDb);
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

        // Grand Company is parsed, so check is skipped. Parsing would've failed earlier already.
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
        Assert.Equal(characterLodestone.RaceClanGender, characterDto.RaceClanGender);
        Assert.Equal(characterLodestone.TownName, characterDto.TownName);
        Assert.Equal(characterLodestone.TownIcon?.ToString(), characterDto.TownIcon);

        CompareGear(characterLodestone.Gear, characterDto);
        CompareAttributes(characterLodestone.Attributes, characterDto);
    }

    private static void CompareGear(CharacterGear gearLodestone, CharacterDto characterDto)
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

    private static void CompareAttributes(CharacterAttributes attributes, CharacterDto characterDto)
    {
        Assert.Equal(attributes.Strength, characterDto.Attributes.Strength);
        Assert.Equal(attributes.Dexterity, characterDto.Attributes.Dexterity);
        Assert.Equal(attributes.Vitality, characterDto.Attributes.Vitality);
        Assert.Equal(attributes.Intelligence, characterDto.Attributes.Intelligence);
        Assert.Equal(attributes.Mind, characterDto.Attributes.Mind);
        Assert.Equal(attributes.CriticalHitRate, characterDto.Attributes.CriticalHitRate);
        Assert.Equal(attributes.Determination, characterDto.Attributes.Determination);
        Assert.Equal(attributes.DirectHitRate, characterDto.Attributes.DirectHitRate);
        Assert.Equal(attributes.Defense, characterDto.Attributes.Defense);
        Assert.Equal(attributes.MagicDefense, characterDto.Attributes.MagicDefense);
        Assert.Equal(attributes.AttackPower, characterDto.Attributes.AttackPower);
        Assert.Equal(attributes.SkillSpeed, characterDto.Attributes.SkillSpeed);
        Assert.Equal(attributes.AttackMagicPotency, characterDto.Attributes.AttackMagicPotency);
        Assert.Equal(attributes.HealingMagicPotency, characterDto.Attributes.HealingMagicPotency);
        Assert.Equal(attributes.SpellSpeed, characterDto.Attributes.SpellSpeed);
        Assert.Equal(attributes.Tenacity, characterDto.Attributes.Tenacity);
        Assert.Equal(attributes.Piety, characterDto.Attributes.Piety);
        Assert.Equal(attributes.Hp, characterDto.Attributes.Hp);
        Assert.Equal(attributes.MpGpCp, characterDto.Attributes.MpGpCp);
        Assert.Equal(attributes.MpGpCpParameterName, characterDto.Attributes.MpGpCpParameterName);
    }

    #endregion
}