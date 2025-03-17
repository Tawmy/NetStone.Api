using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
using NetStone.Cache.Services;
using NetStone.Common.DTOs.Character;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;
using NetStone.Model.Parseables.Character;
using NetStone.Test.DataGenerators;
using NetStone.Test.Fixtures;
using NSubstitute;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using CharacterClassJob = NetStone.Model.Parseables.Character.ClassJob.CharacterClassJob;

namespace NetStone.Test.Tests;

public class CharacterComparisons(ITestOutputHelper testOutputHelper, CharacterComparisonsFixture fixture)
    : TestBed<CharacterComparisonsFixture>(testOutputHelper, fixture)
{
    private readonly ICharacterEventService _characterEventService =
        fixture.GetService<ICharacterEventService>(testOutputHelper)!;

    private readonly CharacterClassJobsService _classJobsService =
        fixture.GetService<CharacterClassJobsService>(testOutputHelper)!;

    private readonly LodestoneClient _lodestoneClient = fixture.GetService<LodestoneClient>(testOutputHelper)!;
    private readonly IMapper _mapper = fixture.GetService<IMapper>(testOutputHelper)!;

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task CharacterV2V3Match(string lodestoneId)
    {
        var (characterServiceV3, characterServiceV2) = CreateServices();

        var v3 = await characterServiceV3.GetCharacterAsync(lodestoneId, null, false);
        Assert.NotNull(v3);

        var v2 = await characterServiceV2.GetCharacterAsync(lodestoneId, null);
        Assert.NotNull(v2);

        Assert.Equal(v3.Id, v2.Id);
        Assert.Equal(v3.Name, v2.Name);
        Assert.Equal(v3.Server, v2.Server);
        Assert.Equal(v3.Title, v2.Title);
        Assert.Equal(v3.Avatar, v2.Avatar);
        Assert.Equal(v3.Portrait, v2.Portrait);
        Assert.Equal(v3.Bio, v2.Bio);
        Assert.Equal(v3.Nameday, v2.Nameday);

        Assert.Equal(v3.ActiveClassJob, v2.ActiveClassJob);
        Assert.Equal(v3.ActiveClassJobLevel, v2.ActiveClassJobLevel);
        Assert.Equal(v3.ActiveClassJobIcon, v2.ActiveClassJobIcon);

        Assert.Equal(v3.GrandCompany, v2.GrandCompany);
        Assert.Equal(v3.GrandCompanyRank, v2.GrandCompanyRank);

        Assert.Equal(v3.FreeCompany, v2.FreeCompany);

        Assert.Equal(v3.GuardianDeityName, v2.GuardianDeityName);
        Assert.Equal(v3.GuardianDeityIcon, v2.GuardianDeityIcon);

        Assert.Equal(v3.PvpTeam, v2.PvpTeam);
        Assert.Equal(v3.Race, v2.Race);
        Assert.Equal(v3.Tribe, v2.Tribe);
        Assert.Equal(v3.Gender, v2.Gender);

        Assert.Equal(v3.TownName, v2.TownName);
        Assert.Equal(v3.TownIcon, v2.TownIcon);

        foreach (var gearPieceV3 in v3.Gear)
        {
            var gearPieceV2 = v2.Gear.FirstOrDefault(x => x.Slot == gearPieceV3.Slot);
            Assert.NotNull(gearPieceV2);

            Assert.Equal(gearPieceV3.ItemName, gearPieceV2.ItemName);
            Assert.Equal(gearPieceV3.ItemLevel, gearPieceV2.ItemLevel);
            Assert.Equal(gearPieceV3.ItemDatabaseLink, gearPieceV2.ItemDatabaseLink);
            Assert.Equal(gearPieceV3.IsHq, gearPieceV2.IsHq);
            Assert.Equal(gearPieceV3.StrippedItemName, gearPieceV2.StrippedItemName);
            Assert.Equal(gearPieceV3.GlamourDatabaseLink, gearPieceV2.GlamourDatabaseLink);
            Assert.Equal(gearPieceV3.GlamourName, gearPieceV2.GlamourName);
            Assert.Equal(gearPieceV3.CreatorName, gearPieceV2.CreatorName);

            foreach (var (materiaV3, slot) in gearPieceV3.Materia.Select((x, i) => (x, i)))
            {
                Assert.Equal(materiaV3, gearPieceV2.Materia.ElementAtOrDefault(slot));
            }
        }

        Assert.Equal(v3.Attributes.Count, v2.Attributes.Count);

        foreach (var (attributeV3, valueV3) in v3.Attributes)
        {
            Assert.True(v2.Attributes.TryGetValue(attributeV3, out var valueV2));
            Assert.Equal(valueV3, valueV2);
        }
    }

    [Theory]
    [ClassData(typeof(CharacterTestsDataGenerator))]
    public async Task CharacterClassJobsV2V3Match(string lodestoneId)
    {
        var (characterServiceV3, characterServiceV2) = CreateServices();

        var v3 = await characterServiceV3.GetCharacterClassJobsAsync(lodestoneId, null, false);
        Assert.NotNull(v3);

        var v2 = await characterServiceV2.GetCharacterClassJobsAsync(lodestoneId, null);
        Assert.NotNull(v2);

        foreach (var classJobDtoV3 in v3.Unlocked)
        {
            Assert.Contains(classJobDtoV3, v2.Unlocked);
        }

        foreach (var classJobDtoV3 in v3.Locked)
        {
            Assert.Contains(classJobDtoV3, v2.Locked);
        }

        Assert.Equal(v3.Cached, v2.Cached);
        Assert.NotNull(v3.LastUpdated);
        Assert.NotNull(v2.LastUpdated);
    }

    private (ICharacterServiceV3 characterServiceV3, ICharacterServiceV2 characterServiceV2) CreateServices()
    {
        var characterCachingService = Substitute.For<ICharacterCachingService>();

        characterCachingService.CacheCharacterAsync(Arg.Any<string>(), Arg.Any<LodestoneCharacter>())
            .Returns(x =>
            {
                var db = _mapper.Map<Character>((LodestoneCharacter)x[1]);
                db.LodestoneId = (string)x[0];
                return _mapper.Map<CharacterDtoV3>(db);
            });

        characterCachingService.GetCharacterClassJobsAsync(Arg.Any<string>()).Returns(([], null));

        characterCachingService.CacheCharacterClassJobsAsync(Arg.Any<string>(), Arg.Any<CharacterClassJob>())
            .Returns(x =>
            {
                var db = _classJobsService.GetCharacterClassJobs(((CharacterClassJob)x[1]).ClassJobDict, []).ToList();
                db.ForEach(y => y.CharacterLodestoneId = (string)x[0]);
                return db.Select(y => _mapper.Map<CharacterClassJobDto>(y)).ToList();
            });

        var characterServiceV3 =
            new CharacterServiceV3(_lodestoneClient, characterCachingService, _characterEventService, _mapper);


        var characterServiceV2 =
            new CharacterServiceV2(_lodestoneClient, characterCachingService, _characterEventService, _mapper);

        return (characterServiceV3, characterServiceV2);
    }
}