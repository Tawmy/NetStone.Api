using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterGearMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterGearMappingExtensions));

    public static CharacterGearDto ToDto(this CharacterGear source)
    {
        using var activity = ActivitySource.StartActivity();

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