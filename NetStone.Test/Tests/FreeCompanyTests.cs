using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
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
    private readonly IAutoMapperService _mapper = fixture.GetService<IAutoMapperService>(testOutputHelper)!;
    private readonly INetStoneService _netStoneService = fixture.GetService<INetStoneService>(testOutputHelper)!;

    #region Free Company Members

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ApiFreeCompanyMembersMatchDto(string lodestoneId)
    {
        var membersLodestoneOuter = await _netStoneService.GetFreeCompanyMembers(lodestoneId);
        Assert.NotNull(membersLodestoneOuter);
        Assert.True(membersLodestoneOuter.HasResults);

        var membersLodestone = membersLodestoneOuter.Members.ToList();

        if (membersLodestoneOuter.NumPages > 1)
        {
            for (var i = 2; i <= membersLodestoneOuter.NumPages; i++)
            {
                var lodestoneMembersOuter2 = await _netStoneService.GetFreeCompanyMembers(lodestoneId, i);
                Assert.NotNull(lodestoneMembersOuter2);
                Assert.True(lodestoneMembersOuter2.HasResults);

                membersLodestone.AddRange(lodestoneMembersOuter2.Members);
            }
        }

        foreach (var memberLodestone in membersLodestone)
        {
            var memberDb = _mapper.Map<FreeCompanyMember>(memberLodestone);
            Assert.NotNull(memberDb);

            memberDb.FreeCompanyLodestoneId = lodestoneId; // also set manually in code

            var memberDto = _mapper.Map<FreeCompanyMemberDto>(memberDb);
            Assert.NotNull(memberDto);

            Assert.Equal(memberLodestone.Id, memberDto.LodestoneId);
            Assert.Equal(lodestoneId, memberDto.FreeCompanyLodestoneId);
            Assert.Equal(memberLodestone.Name, memberDto.Name);
            Assert.Equal(memberLodestone.Rank, memberDto.Rank);
            Assert.Equal(memberLodestone.RankIcon?.ToString(), memberDto.RankIcon);
            Assert.Equal(memberLodestone.Server, memberDto.Server);
            Assert.Equal(memberLodestone.Datacenter, memberDto.DataCenter);
            Assert.Equal(memberLodestone.Avatar?.ToString(), memberDto.Avatar);
        }
    }

    #endregion

    #region Free Company

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task ApiFreeCompanyMatchesDto(string lodestoneId)
    {
        var freeCompanyLodestone = await _netStoneService.GetFreeCompany(lodestoneId);
        Assert.NotNull(freeCompanyLodestone);

        var freeCompanyDb = _mapper.Map<FreeCompany>(freeCompanyLodestone);
        Assert.NotNull(freeCompanyDb);

        var freeCompanyDto = _mapper.Map<FreeCompanyDtoV3>(freeCompanyDb);
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

        if (!string.IsNullOrWhiteSpace(freeCompanyLodestone.GrandCompany))
        {
            Assert.NotEqual(GrandCompany.NoAffiliation, freeCompanyDto.GrandCompany);
        }

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

    private static void CompareFocus(LodestoneFreeCompany freeCompanyLodestone, FreeCompanyDtoV3 freeCompanyDto)
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
            var focusDto = freeCompanyDto.Focus.FirstOrDefault(x => x.Type.ToString() == fcLodestonePropInfo.Name);
            Assert.NotNull(focusDto);

            // name already got implicitly compared in FirstOrDefault call, only compare icon
            Assert.Equal(focusLodestoneEntry.Icon?.ToString(), focusDto.Icon);
        }
    }

    #endregion
}