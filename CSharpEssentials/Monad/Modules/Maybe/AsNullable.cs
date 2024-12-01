namespace CSharpEssentials;

public readonly partial struct Maybe<T>
{
    /// <summary>
    /// Converts a Maybe monad to a nullable value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public T? AsNullable()
    {
        if (TryGetValue(out var result))
            return result;
        return default;
    }
}
