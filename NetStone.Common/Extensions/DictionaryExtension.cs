namespace NetStone.Common.Extensions;

public static class DictionaryExtension
{
    public static void AddIfKeyNotNull<T1, T2>(this IDictionary<T1, T2> dict, T1? key, T2 value)
    {
        if (key is not null)
        {
            dict.Add(key, value);
        }
    }

    public static void AddIfValueNotNull<T1, T2>(this IDictionary<T1, T2> dict, T1 key, T2? value)
    {
        if (value is not null)
        {
            dict.Add(key, value);
        }
    }
}