using System;

namespace CSharpEssentials;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Tries to find a value in a dictionary.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Maybe<V> TryFind<K, V>(this IReadOnlyDictionary<K, V> dict, K key)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        return Maybe.None;
    }
}
