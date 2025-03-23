using NetStone.Cache.Db.Models;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneFreeCompanyMembersEntryMappingExtensions
{
    public static FreeCompanyMember ToDb(this FreeCompanyMembersEntry source, string freeCompanyLodestoneId)
    {
        return new FreeCompanyMember
        {
            CharacterLodestoneId = source.Id,
            FreeCompanyLodestoneId = freeCompanyLodestoneId,

            Name = source.Name,
            Rank = source.Rank,
            RankIcon = source.RankIcon?.ToString(),
            Server = source.Server,
            DataCenter = source.Datacenter,
            Avatar = source.Avatar?.ToString() ??
                     throw new InvalidOperationException($"{nameof(source.Avatar)} must not be null.")
        };
    }

    public static void ToDb(this FreeCompanyMembersEntry source, FreeCompanyMember target)
    {
        target.Name = source.Name;
        target.Rank = source.Rank;
        target.RankIcon = source.RankIcon?.ToString();
        target.Avatar = source.Avatar?.ToString() ??
                        throw new InvalidOperationException($"{nameof(source.Avatar)} must not be null.");
    }
}