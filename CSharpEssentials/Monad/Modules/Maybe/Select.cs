namespace CSharpEssentials;

public readonly partial struct Maybe<T>
{
    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public Maybe<K> Select<K>(Func<T, K> selector)
    {
        return Map(selector);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public Maybe<K> SelectMany<K>(Func<T, Maybe<K>> selector)
    {
        return Bind(selector);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="selector"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public Maybe<V> SelectMany<U, V>(
        Func<T, Maybe<U>> selector,
        Func<T, U, V> project)
    {
        return this.GetValueOrDefault(
            x => selector(x).GetValueOrDefault(u => project(x, u), Maybe<V>.None),
            Maybe<V>.None);
    }
}
