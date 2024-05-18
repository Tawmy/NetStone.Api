using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Common.Enums;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Test.DataGenerators;
using NetStone.Test.Fixtures;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Tests;

public class FreeCompanyTests(ITestOutputHelper testOutputHelper, FreeCompanyTestsFixture fixture)
    : TestBed<FreeCompanyTestsFixture>(testOutputHelper, fixture)
{
    private readonly LodestoneClient _client = fixture.GetService<LodestoneClient>(testOutputHelper)!;

    private readonly IMapper _mapper = fixture.GetService<IMapper>(testOutputHelper)!;

    #region Free Company

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ApiFreeCompanyMatchesDto(string lodestoneId)
    {
        var freeCompanyLodestone = await _client.GetFreeCompany(lodestoneId);
        Assert.NotNull(freeCompanyLodestone);

        var freeCompanyDb = _mapper.Map<FreeCompany>(freeCompanyLodestone);
        Assert.NotNull(freeCompanyDb);

        var freeCompanyDto = _mapper.Map<FreeCompanyDto>(freeCompanyDb);
        Assert.NotNull(freeCompanyDto);

        Assert.Equal(freeCompanyLodestone.Name, freeCompanyDto.Name);
        Assert.Equal(freeCompanyLodestone.Id, freeCompanyDto.Id);
        Assert.Equal(freeCompanyLodestone.World, freeCompanyDto.World);
        Assert.Equal(freeCompanyLodestone.Tag, freeCompanyDto.Tag);
        Assert.Equal(freeCompanyLodestone.Slogan, freeCompanyDto.Slogan);

        Assert.Equal(freeCompanyLodestone.CrestLayers.TopLayer?.ToString(), freeCompanyDto.CrestLayers.TopLayer);
        Assert.Equal(freeCompanyLodestone.CrestLayers.MiddleLayer?.ToString(), freeCompanyDto.CrestLayers.MiddleLayer);
        Assert.Equal(freeCompanyLodestone.CrestLayers.BottomLayer?.ToString(), freeCompanyDto.CrestLayers.BottomLayer);

        Assert.Equal(freeCompanyLodestone.Formed, freeCompanyDto.Formed);
        // Grand Company is parsed, so check is skipped. Parsing would've failed earlier already.

        Assert.Equal(freeCompanyLodestone.Rank, freeCompanyDto.Rank);
        Assert.Equal(freeCompanyLodestone.RankingMonthly, freeCompanyDto.RankingMonthly);
        Assert.Equal(freeCompanyLodestone.RankingWeekly, freeCompanyDto.RankingWeekly);

        Assert.Equal(freeCompanyLodestone.Recruitment, freeCompanyDto.Recruitment);
        Assert.Equal(freeCompanyLodestone.ActiveMemberCount, freeCompanyDto.ActiveMemberCount);
        Assert.Equal(freeCompanyLodestone.ActiveState, freeCompanyDto.ActiveState);

        Assert.Equal(freeCompanyLodestone.Estate?.Name, freeCompanyDto.Estate?.Name);
        Assert.Equal(freeCompanyLodestone.Estate?.Greeting, freeCompanyDto.Estate?.Greeting);
        Assert.Equal(freeCompanyLodestone.Estate?.Plot, freeCompanyDto.Estate?.Plot);

        CompareFocus(freeCompanyLodestone, freeCompanyDto);

        Assert.Equal(freeCompanyLodestone.Reputation.Maelstrom.Rank,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.Maelstrom).Rank);
        Assert.Equal(freeCompanyLodestone.Reputation.Maelstrom.Progress,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.Maelstrom).Progress);
        Assert.Equal(freeCompanyLodestone.Reputation.Adders.Rank,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.OrderOfTheTwinAdder).Rank);
        Assert.Equal(freeCompanyLodestone.Reputation.Adders.Progress,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.OrderOfTheTwinAdder).Progress);
        Assert.Equal(freeCompanyLodestone.Reputation.Flames.Rank,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.ImmortalFlames).Rank);
        Assert.Equal(freeCompanyLodestone.Reputation.Flames.Progress,
            freeCompanyDto.Reputation.Single(x => x.GrandCompany == GrandCompany.ImmortalFlames).Progress);
    }

    private static void CompareFocus(LodestoneFreeCompany freeCompanyLodestone, FreeCompanyDto freeCompanyDto)
    {
        if (freeCompanyLodestone.Focus?.HasFocus is not true)
        {
            return;
        }

        foreach (var fcLodestonePropInfo in freeCompanyLodestone.Focus.GetType().GetProperties())
        {
            if (fcLodestonePropInfo.PropertyType != typeof(FreeCompanyFocusEntry)) continue;

            // use reflection to retrieve properties as no dict available
            var focusLodestoneEntry = (FreeCompanyFocusEntry)fcLodestonePropInfo.GetValue(freeCompanyLodestone.Focus)!;

            // disabled focus is not part of DTO, skip
            if (!focusLodestoneEntry.IsEnabled) continue;

            // enum values match property names from NetStone, so we can retrieve values by property name
            var focusDto = freeCompanyDto.Focus.FirstOrDefault(x => x.Name == fcLodestonePropInfo.Name);
            Assert.NotNull(focusDto);

            // name already got implicitly compared in FirstOrDefault call, only compare icon
            Assert.Equal(focusLodestoneEntry.Icon?.ToString(), focusDto.Icon);
        }
    }

    #endregion
}