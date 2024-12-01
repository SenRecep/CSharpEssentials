namespace CSharpEssentials;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Tries to get the first value in a sequence.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Maybe<T> TryFirst<T>(this IEnumerable<T> source)
    {
        foreach (var item in source)
            return item;

        return Maybe.None;
    }

    /// <summary>
    /// Tries to get the first value in a sequence that satisfies a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> TryFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
            if (predicate(item))
                return item;

        return Maybe.None;
    }
}
