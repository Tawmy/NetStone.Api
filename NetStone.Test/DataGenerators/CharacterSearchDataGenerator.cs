using NetStone.Common.Enums;
using NetStone.Common.Queries;

namespace NetStone.Test.DataGenerators;

internal sealed class CharacterSearchDataGenerator : TheoryData<CharacterSearchTestData>
{
    public CharacterSearchDataGenerator()
    {
        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen", "Phoenix"),
            1
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen",
                "Phoenix",
                ClassJob: ClassJob.Scholar,
                Tribe: Tribe.Xaela,
                GrandCompany: GrandCompany.Maelstrom,
                Language: Language.Japanese | Language.English | Language.German,
                SortKind: SortKindCharacter.NameAtoZ),
            1
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx Bergen",
                DataCenter: "Light",
                Role: Role.Healer,
                Race: Race.AuRa,
                GrandCompany: GrandCompany.Maelstrom),
            1
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1,
            1
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx"),
            -1,
            2
        ));

        Add(new CharacterSearchTestData
        (
            new CharacterSearchQuery("Alyx", "Phoenix", Tribe: Tribe.Xaela,
                GrandCompany: GrandCompany.Maelstrom, Language: Language.German),
            1
        ));
    }
}

public record CharacterSearchTestData(CharacterSearchQuery Query, int ExpectedResults, int? Page = null);