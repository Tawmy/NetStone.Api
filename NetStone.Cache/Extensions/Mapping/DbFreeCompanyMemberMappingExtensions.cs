using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.FreeCompany;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbFreeCompanyMemberMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbFreeCompanyMemberMappingExtensions));

    public static FreeCompanyMemberDtoV3 ToDto(this FreeCompanyMember source)
    {
        using var activity = ActivitySource.StartActivity();

        return new FreeCompanyMemberDtoV3
        {
            LodestoneId = source.CharacterLodestoneId,
            FreeCompanyLodestoneId = source.FreeCompanyLodestoneId,

            CachedCharacter = source.FullCharacter?.ToDto() is { } cachedCharacter
                ? cachedCharacter with { Cached = true }
                : null,

            Name = source.Name,
            Rank = source.Rank,
            RankIcon = source.RankIcon,
            Server = source.Server,
            DataCenter = source.DataCenter,
            Avatar = source.Avatar
        };
    }
}