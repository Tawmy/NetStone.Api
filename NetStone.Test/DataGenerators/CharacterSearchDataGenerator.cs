using NetStone.Common.Enums;
using NetStone.Common.Queries;

namespace NetStone.Test.DataGenerators;

public class CharacterSearchDataGenerator : TheoryData<SearchTestData>
{
    public CharacterSearchDataGenerator()
    {
        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen", "Phoenix"),
            1
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen",
                "Phoenix",
                ClassJob: ClassJob.Scholar,
                Tribe: Tribe.Xaela,
                GrandCompany: GrandCompany.Maelstrom,
                Language: Language.Japanese | Language.English | Language.German,
                SortKind: SortKind.NameAtoZ),
            1
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen",
                DataCenter: "Light",
                Role: Role.Healer,
                Race: Race.AuRa,
                GrandCompany: GrandCompany.Maelstrom),
            1
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1,
            1
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1,
            2
        ));

        Add(new SearchTestData
        (
            new CharacterSearchQuery("Alyx", "Phoenix", Tribe: Tribe.Xaela,
                GrandCompany: GrandCompany.Maelstrom),
            1
        ));
    }
}

public record SearchTestData(CharacterSearchQuery Query, int ExpectedResults, int? Page = null);