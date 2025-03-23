using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneIconLayersMappingExtensions
{
    public static FreeCompanyCrestDto ToDto(this IconLayers source)
    {
        return new FreeCompanyCrestDto
        {
            TopLayer = source.TopLayer?.ToString(),
            MiddleLayer = source.MiddleLayer?.ToString(),
            BottomLayer = source.BottomLayer?.ToString()
        };
    }
}