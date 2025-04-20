using System.Diagnostics;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneIconLayersMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneIconLayersMappingExtensions));

    public static FreeCompanyCrestDto ToDto(this IconLayers source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompanyCrestDto
        {
            TopLayer = source.TopLayer?.ToString(),
            MiddleLayer = source.MiddleLayer?.ToString(),
            BottomLayer = source.BottomLayer?.ToString()
        };
    }
}