using NetStone.Common.Enums;
using NetStone.Common.Queries;

namespace NetStone.Test.DataGenerators;

internal class FreeCompanySearchDataGenerator : TheoryData<FreeCompanySearchTestData>
{
    public FreeCompanySearchDataGenerator()
    {
        Add(new FreeCompanySearchTestData(
            new FreeCompanySearchQuery("Dust Bunnies",
                "Phoenix",
                IsCommunityFinderRecruiting: false,
                ActiveTimes: ActiveTimes.Always,
                ActiveMembers: ActiveMembers.ThirtyOneToFifty,
                Recruitment: Recruitment.Closed,
                Housing: Housing.EstateBuilt,
                Focus: FreeCompanyFocus.Leveling | FreeCompanyFocus.Casual | FreeCompanyFocus.Dungeons |
                       FreeCompanyFocus.Trials | FreeCompanyFocus.Raids | FreeCompanyFocus.PvP,
                Seeking: Seeking.NotSpecified,
                GrandCompany: GrandCompany.ImmortalFlames,
                SortKind: SortKind.WorldZtoA),
            1
        ));
    }
}

public record FreeCompanySearchTestData(FreeCompanySearchQuery Query, int ExpectedResults, int? Page = null);