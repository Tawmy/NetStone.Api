using AutoMapper;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Test.DataGenerators;
using NetStone.Test.Fixtures;
using NSubstitute;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace NetStone.Test.Tests;

public class FreeCompanyComparisons(ITestOutputHelper testOutputHelper, FreeCompanyComparisonsFixture fixture)
    : TestBed<FreeCompanyComparisonsFixture>(testOutputHelper, fixture)
{
    private readonly IFreeCompanyEventService _freeCompanyEventService =
        fixture.GetService<IFreeCompanyEventService>(testOutputHelper)!;

    private readonly LodestoneClient _lodestoneClient = fixture.GetService<LodestoneClient>(testOutputHelper)!;
    private readonly IMapper _mapper = fixture.GetService<IMapper>(testOutputHelper)!;

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task FreeCompanyV2V3Match(string lodestoneId)
    {
        var (fcCachingService, fcServiceV3, fcServiceV2) = CreateServices();

        fcCachingService.CacheFreeCompanyAsync(Arg.Any<LodestoneFreeCompany>())
            .Returns(x =>
            {
                var lodestoneFreeCompany = (LodestoneFreeCompany)x[0];
                var db = _mapper.Map<FreeCompany>(lodestoneFreeCompany);
                db.LodestoneId = lodestoneFreeCompany.Id;
                return _mapper.Map<FreeCompanyDtoV3>(db);
            });

        var v3 = await fcServiceV3.GetFreeCompanyAsync(lodestoneId, null, false);
        Assert.NotNull(v3);

        var v2 = await fcServiceV2.GetFreeCompanyAsync(lodestoneId, null);
        Assert.NotNull(v2);

        Assert.Equal(v3.Name, v2.Name);
        Assert.Equal(v3.Id, v2.Id);
        Assert.Equal(v3.World, v2.World);
        Assert.Equal(v3.Tag, v2.Tag);
        Assert.Equal(v3.Slogan, v2.Slogan);

        Assert.Equal(v3.CrestLayers, v2.CrestLayers);

        Assert.Equal(v3.Formed, v2.Formed);
        Assert.Equal(v3.GrandCompany, v2.GrandCompany);
        Assert.Equal(v3.Rank, v2.Rank);
        Assert.Equal(v3.RankingMonthly, v2.RankingMonthly);
        Assert.Equal(v3.RankingWeekly, v2.RankingWeekly);
        Assert.Equal(v3.Recruitment, v2.Recruitment);
        Assert.Equal(v3.ActiveMemberCount, v2.ActiveMemberCount);
        Assert.Equal(v3.ActiveState, v2.ActiveState);

        Assert.Equal(v3.Estate, v2.Estate);

        foreach (var focusV3 in v3.Focus)
        {
            Assert.Contains(focusV3, v2.Focus);
        }

        foreach (var reputationV3 in v3.Reputation)
        {
            Assert.Contains(reputationV3, v2.Reputation);
        }

        Assert.Equal(v3.Cached, v2.Cached);
        Assert.NotNull(v3.LastUpdated);
        Assert.NotNull(v2.LastUpdated);
    }

    private (IFreeCompanyCachingService fcCachingService, IFreeCompanyServiceV3 fcServiceV3,
        IFreeCompanyServiceV2 fcServiceV2) CreateServices()
    {
        var fcCachingService = Substitute.For<IFreeCompanyCachingService>();

        var fcServiceV3 =
            new FreeCompanyServiceV3(_lodestoneClient, fcCachingService, _freeCompanyEventService, _mapper);

        var fcServiceV2 =
            new FreeCompanyServiceV2(_lodestoneClient, fcCachingService, _freeCompanyEventService, _mapper);

        return (fcCachingService, fcServiceV3, fcServiceV2);
    }
}