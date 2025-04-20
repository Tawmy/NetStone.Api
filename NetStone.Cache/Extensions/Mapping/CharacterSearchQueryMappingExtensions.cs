using System.Diagnostics;
using NetStone.Search.Character;
using NetStone.StaticData;
using CharacterSearchQuery = NetStone.Common.Queries.CharacterSearchQuery;

namespace NetStone.Cache.Extensions.Mapping;

public static class CharacterSearchQueryMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(CharacterSearchQueryMappingExtensions));

    public static Search.Character.CharacterSearchQuery ToNetStone(this CharacterSearchQuery source)
    {
        using var activity = ActivitySource.StartActivity();

        return new Search.Character.CharacterSearchQuery
        {
            CharacterName = source.CharacterName,
            World = source.World ?? string.Empty,
            DataCenter = source.DataCenter ?? string.Empty,
            Role = source.Role is not null ? Enum.Parse<Role>(source.Role.ToString()!) : Role.None,
            ClassJob = source.ClassJob is not null ? Enum.Parse<ClassJob>(source.ClassJob.ToString()!) : ClassJob.None,
            Race = source.Race is not null ? Enum.Parse<Race>(source.Race.ToString()!) : Race.None,
            Tribe = source.Tribe is not null ? Enum.Parse<Tribe>(source.Tribe.ToString()!) : Tribe.None,
            GrandCompany = source.GrandCompany is not null
                ? Enum.Parse<GrandCompany>(source.GrandCompany.ToString()!)
                : GrandCompany.None,
            Language = source.Language is not null ? (Language)source.Language : Language.None,
            SortKind = source.SortKind is not null
                ? Enum.Parse<SortKind>(source.SortKind.ToString()!)
                : SortKind.NameAtoZ
        };
    }
}