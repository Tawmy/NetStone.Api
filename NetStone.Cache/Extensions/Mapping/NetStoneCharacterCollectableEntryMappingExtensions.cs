using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.Character.Collectable;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneCharacterCollectableEntryMappingExtensions
{
    public static CharacterMinion ToDbMinion(this CharacterCollectableEntry source, string characterLodestoneId)
    {
        return new CharacterMinion
        {
            CharacterLodestoneId = characterLodestoneId,
            Name = source.Name
        };
    }

    public static CharacterMount ToDbMount(this CharacterCollectableEntry source, string characterLodestoneId)
    {
        return new CharacterMount
        {
            CharacterLodestoneId = characterLodestoneId,
            Name = source.Name
        };
    }
}