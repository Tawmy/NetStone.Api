using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterFreeCompanyMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterFreeCompanyMappingExtensions));

    public static CharacterFreeCompanyDto ToDto(this CharacterFreeCompany source)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterFreeCompanyDto
        {
            Id = source.LodestoneId,
            Name = source.Name,
            Link = source.Link,
            IconLayers = new FreeCompanyCrestDto
            {
                TopLayer = source.TopLayer,
                MiddleLayer = source.MiddleLayer,
                BottomLayer = source.BottomLayer
            }
        };
    }
}