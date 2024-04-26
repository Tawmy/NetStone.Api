using System.Collections;

namespace NetStone.Common.Extensions;

public static class ListExtension
{
    public static void AddIfNotNull<T>(this IList list, T? obj)
    {
        if (obj is not null)
        {
            list.Add(obj);
        }
    }
}