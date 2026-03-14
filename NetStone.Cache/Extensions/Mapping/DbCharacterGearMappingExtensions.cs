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
            ItemIconLink = source.ItemIconLink,
            IsHq = source.IsHq,
            StrippedItemName = source.StrippedItemName,
            GlamourDatabaseLink = source.GlamourDatabaseLink,
            GlamourIconLink = source.GlamourIconLink,
            GlamourName = source.GlamourName,
            CreatorName = source.CreatorName,
            Materia = source.GetMateriaList(),
            Dye1 = ToDyeDto(source.Dye1Name, source.Dye1Color, source.Dye1DatabaseLink),
            Dye2 = ToDyeDto(source.Dye2Name, source.Dye2Color, source.Dye2DatabaseLink)
        };
    }

    private static CharacterGearDyeDto? ToDyeDto(string? name, string? color, string? databaseLink)
    {
        if (string.IsNullOrEmpty(name)) return null;

        if (string.IsNullOrEmpty(color)) throw new InvalidOperationException($"dye {nameof(color)} must not be null");
        if (databaseLink is null) throw new InvalidOperationException($"dye {nameof(databaseLink)} must not be null");

        return new CharacterGearDyeDto
        {
            Name = name,
            Color = color,
            DatabaseLink = databaseLink
        };
    }
}