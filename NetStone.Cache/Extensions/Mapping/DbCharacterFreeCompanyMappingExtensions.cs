using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;
using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterFreeCompanyMappingExtensions
{
    public static CharacterFreeCompanyDto ToDto(this CharacterFreeCompany source)
    {
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