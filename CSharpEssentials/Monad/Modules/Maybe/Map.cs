namespace CSharpEssentials;

public readonly partial struct Maybe<T>
{
    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public  async Task<Maybe<K>> MapAsync<K>(Func<T, Task<K>> selector, CancellationToken cancellationToken = default)
    {
        if (HasNoValue)
            return Maybe.None;

        return await selector(Value).WithCancellation(cancellationToken);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="valueTask"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public  async ValueTask<Maybe<K>> MapAsync<K>(Func<T, ValueTask<K>> valueTask, CancellationToken cancellationToken = default)
    {
        if (HasNoValue)
            return Maybe.None;

        return await valueTask(Value).WithCancellation(cancellationToken);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public  Maybe<K> Map<K>(Func<T, K> selector)
    {
        if (HasNoValue)
            return Maybe.None;

        return selector(Value);
    }
}

public static partial class MaybeExtensions
{
    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Maybe<K>> MapAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, K> selector, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.Map(selector);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Maybe<K>> MapAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, Task<K>> selector, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.MapAsync(selector, cancellationToken);
    }

    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="valueTask"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async ValueTask<Maybe<K>> MapAsync<T, K>(this ValueTask<Maybe<T>> valueTask, Func<T, K> selector, CancellationToken cancellationToken = default)
    {
        Maybe<T> maybe = await valueTask;
        return maybe.Map(selector);
    }


    /// <summary>
    /// Maps the value of the Maybe to a new value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="valueTask"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async ValueTask<Maybe<K>> MapAsync<T, K>(this ValueTask<Maybe<T>> maybeTask, Func<T, ValueTask<K>> valueTask, CancellationToken cancellationToken = default)
    {
        Maybe<T> maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.MapAsync(valueTask, cancellationToken);
    }
}