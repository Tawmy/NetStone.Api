using Microsoft.Extensions.Logging;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Data.Interfaces;
using NetStone.Data.Services;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;
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

    private readonly IAutoMapperService _mapper = fixture.GetService<IAutoMapperService>(testOutputHelper)!;

    private readonly INetStoneService _netStoneService = fixture.GetService<INetStoneService>(testOutputHelper)!;

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task FreeCompanyV2V3Match(string lodestoneId)
    {
        var (fcCachingServiceV3, fcCachingServiceV2, fcServiceV3, fcServiceV2) = CreateServices();

        fcCachingServiceV3.CacheFreeCompanyAsync(Arg.Any<LodestoneFreeCompany>())
            .Returns(x =>
            {
                var lodestoneFreeCompany = (LodestoneFreeCompany)x[0];
                var db = lodestoneFreeCompany.ToDb();
                return db.ToDto();
            });

        fcCachingServiceV2.CacheFreeCompanyAsync(Arg.Any<LodestoneFreeCompany>())
            .Returns(x =>
            {
                var lodestoneFreeCompany = (LodestoneFreeCompany)x[0];
                var db = _mapper.Map<FreeCompany>(lodestoneFreeCompany);
                return _mapper.Map<FreeCompanyDtoV2>(db);
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

    [Theory]
    [ClassData(typeof(FreeCompanyTestsDataGenerator))]
    public async Task FreeCompanyMembersV2V3Match(string lodestoneId)
    {
        var (fcCachingServiceV3, fcCachingServiceV2, fcServiceV3, fcServiceV2) = CreateServices();

        fcCachingServiceV3.GetFreeCompanyMembersAsync(Arg.Any<string>()).Returns(([], null));
        fcCachingServiceV2.GetFreeCompanyMembersAsync(Arg.Any<string>()).Returns(([], null));

        fcCachingServiceV3
            .CacheFreeCompanyMembersAsync(Arg.Any<string>(), Arg.Any<ICollection<FreeCompanyMembersEntry>>())
            .Returns(x =>
            {
                var dbs = ((ICollection<FreeCompanyMembersEntry>)x[1]).Select(y => y.ToDb((string)x[0]));
                return dbs.Select(y => y.ToDto()).ToList();
            });

        fcCachingServiceV2
            .CacheFreeCompanyMembersAsync(Arg.Any<string>(), Arg.Any<ICollection<FreeCompanyMembersEntry>>())
            .Returns(x =>
            {
                var dbs = ((ICollection<FreeCompanyMembersEntry>)x[1]).Select(y =>
                {
                    var newDb = _mapper.Map<FreeCompanyMember>(y);
                    newDb.FreeCompanyLodestoneId = (string)x[0];
                    return newDb;
                });

                return dbs.Select(_mapper.Map<FreeCompanyMemberDtoV2>).ToList();
            });

        var v3 = await fcServiceV3.GetFreeCompanyMembersAsync(lodestoneId, null, false);
        Assert.NotNull(v3);

        var v2 = await fcServiceV2.GetFreeCompanyMembersAsync(lodestoneId, null);
        Assert.NotNull(v2);

        foreach (var memberV3 in v3.Members)
        {
            var memberV2 = v2.Members.FirstOrDefault(x => x.LodestoneId == memberV3.LodestoneId);
            Assert.NotNull(memberV2);

            Assert.Equal(memberV3.FreeCompanyLodestoneId, memberV2.FreeCompanyLodestoneId);
            Assert.Equal(memberV3.Name, memberV2.Name);
            Assert.Equal(memberV3.Rank, memberV2.Rank);
            Assert.Equal(memberV3.RankIcon, memberV2.RankIcon);
            Assert.Equal(memberV3.Server, memberV2.Server);
            Assert.Equal(memberV3.DataCenter, memberV2.DataCenter);

            // remove query parameters from urls
            var avatarV3 = new Uri(memberV3.Avatar).GetLeftPart(UriPartial.Path);
            var avatarV2 = new Uri(memberV2.Avatar).GetLeftPart(UriPartial.Path);
            Assert.Equal(avatarV3, avatarV2);
        }

        Assert.Equal(v3.Cached, v2.Cached);
        Assert.NotNull(v3.LastUpdated);
        Assert.NotNull(v2.LastUpdated);
    }

    private (IFreeCompanyCachingServiceV3 fcCachingServiceV3, IFreeCompanyCachingServiceV2 fcCachingServiceV2,
        IFreeCompanyServiceV3 fcServiceV3, FreeCompanyServiceV2 fcServiceV2) CreateServices()
    {
        var fcCachingServiceV3 = Substitute.For<IFreeCompanyCachingServiceV3>();
        var fcCachingServiceV2 = Substitute.For<IFreeCompanyCachingServiceV2>();

        var loggerV3 = Substitute.For<ILogger<FreeCompanyServiceV3>>();

        var fcServiceV3 =
            new FreeCompanyServiceV3(_netStoneService, fcCachingServiceV3, _freeCompanyEventService, loggerV3);

        var fcServiceV2 =
            new FreeCompanyServiceV2(_netStoneService, fcCachingServiceV2, _freeCompanyEventService, _mapper);

        return (fcCachingServiceV3, fcCachingServiceV2, fcServiceV3, fcServiceV2);
    }
}