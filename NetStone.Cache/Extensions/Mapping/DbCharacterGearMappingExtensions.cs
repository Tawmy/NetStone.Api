using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterGearMappingExtensions
{
    public static CharacterGearDto ToDto(this CharacterGear source)
    {
        return new CharacterGearDto
        {
            Slot = source.Slot,
            ItemName = source.ItemName,
            ItemLevel = source.ItemLevel,
            ItemDatabaseLink = source.ItemDatabaseLink,
            IsHq = source.IsHq,
            StrippedItemName = source.StrippedItemName,
            GlamourDatabaseLink = source.GlamourDatabaseLink,
            GlamourName = source.GlamourName,
            CreatorName = source.CreatorName,
            Materia = source.GetMateriaList()
        };
    }
}