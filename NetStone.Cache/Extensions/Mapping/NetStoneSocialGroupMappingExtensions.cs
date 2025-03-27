using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneSocialGroupMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneSocialGroupMappingExtensions));

    /// <remarks>Attach this directly to the character so FK is set by EF</remarks>
    public static CharacterFreeCompany ToDb(this SocialGroup x)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterFreeCompany
        {
            CharacterId = 0, // set automatically by EF
            LodestoneId = x.Id ?? throw new InvalidOperationException($"{nameof(x.Id)} must not be null."),
            Name = x.Name,
            Link = x.Link?.ToString() ?? throw new InvalidOperationException($"{nameof(x.Link)} must not be null."),
            TopLayer = x.IconLayers.TopLayer?.ToString(),
            MiddleLayer = x.IconLayers.MiddleLayer?.ToString(),
            BottomLayer = x.IconLayers.BottomLayer?.ToString()
        };
    }

    public static void ToDb(this SocialGroup source, CharacterFreeCompany target)
    {
        using var activity = ActivitySource.StartActivity();

        target.LodestoneId = source.Id ?? throw new InvalidOperationException($"{nameof(source.Id)} must not be null.");
        target.Name = source.Name;
        target.Link = source.Link?.ToString() ??
                      throw new InvalidOperationException($"{nameof(source.Link)} must not be null.");
        target.TopLayer = source.IconLayers.TopLayer?.ToString();
        target.MiddleLayer = source.IconLayers.MiddleLayer?.ToString();
        target.BottomLayer = source.IconLayers.BottomLayer?.ToString();
    }
}