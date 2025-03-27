using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterCollectableEntryMappingExtensions
{
    private static readonly ActivitySource ActivitySource =
        new(nameof(NetStoneCharacterCollectableEntryMappingExtensions));

    public static CharacterMinion ToDbMinion(this CharacterCollectableEntry source, string characterLodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterMinion
        {
            CharacterLodestoneId = characterLodestoneId,
            Name = source.Name
        };
    }

    public static CharacterMount ToDbMount(this CharacterCollectableEntry source, string characterLodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterMount
        {
            CharacterLodestoneId = characterLodestoneId,
            Name = source.Name
        };
    }
}