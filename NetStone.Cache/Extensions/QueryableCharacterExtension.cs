using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Extensions;

internal static class QueryableCharacterExtension
{
    public static IQueryable<Character> IncludeAll(this IQueryable<Character> queryable)
    {
        return queryable
            .Include(x => x.FreeCompany)
            .Include(x => x.Gear);
    }
}