namespace CSharpEssentials;

public readonly partial struct Maybe<T>
{
    /// <summary>
    /// Binds the specified selector to the Maybe monad.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public  Task<Maybe<K>> BindAsync<K>(
            Func<T, Task<Maybe<K>>> selector,
            CancellationToken cancellationToken = default) =>
                HasNoValue ? Maybe<K>.None.AsTask() : selector(Value).WithCancellation(cancellationToken);

    /// <summary>
    /// Binds the specified selector to the Maybe monad.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    public  Maybe<K> Bind<K>(
        Func<T, Maybe<K>> selector) =>
            HasNoValue ? Maybe<K>.None : selector(Value);

    /// <summary>
    /// Binds the specified selector to the Maybe monad.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="selector"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Maybe<K> Bind<K, TContext>(
        Func<T, TContext, Maybe<K>> selector,
        TContext context
    ) => Bind((value) => selector(value, context));
}

public static partial class MaybeExtensions
{
    /// <summary>
    /// Binds the specified selector to the Maybe monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static async Task<Maybe<K>> BindAsync<T, K>(
        this Task<Maybe<T>> maybeTask,
            Func<T, Maybe<K>> selector,
            CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.Bind(selector);
    }
    /// <summary>
    /// Binds the specified selector to the Maybe monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static async ValueTask<Maybe<K>> BindAsync<T, K>(
        this ValueTask<Maybe<T>> maybeTask,
            Func<T, ValueTask<Maybe<K>>> selector,
            CancellationToken cancellationToken = default)
    {
        Maybe<T> maybe = await maybeTask.WithCancellation(cancellationToken);
        ValueTask<Maybe<K>> result = maybe.HasNoValue ? Maybe<K>.None.AsValueTask() : selector(maybe.Value).WithCancellation(cancellationToken);
        return await result;
    }
}
