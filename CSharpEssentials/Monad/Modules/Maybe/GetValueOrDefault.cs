using System;

namespace CSharpEssentials;

public readonly partial struct Maybe<T>
{
    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> GetValueOrDefaultAsync(Func<Task<T>> defaultValue, CancellationToken cancellationToken = default)
    {
        if (HasNoValue)
            return await defaultValue().WithCancellation(cancellationToken);

        return Value;
    }

    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<K> GetValueOrDefaultAsync<K>(Func<T, K> selector,
        Func<Task<K>> defaultValue, CancellationToken cancellationToken = default)
    {
        if (HasNoValue)
            return await defaultValue().WithCancellation(cancellationToken);

        return selector(Value);
    }

    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="selector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public  async Task<K> GetValueOrDefaultAsync< K>(Func<T, Task<K>> selector,
        K defaultValue, CancellationToken cancellationToken = default)
    {
        if (HasNoValue)
            return defaultValue;

        return await selector(Value).WithCancellation(cancellationToken);
    }
}

public static partial class MaybeExtensions
{
    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<T> GetValueOrDefaultAsync<T>(this Task<Maybe<T>> maybeTask, Func<T> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(defaultValue);
    }
    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<K> GetValueOrDefaultAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, K> selector, K defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(selector, defaultValue);
    }
    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybeTask"></param>
    /// <param name="selector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<K> GetValueOrDefaultAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, K> selector, Func<K> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(selector, defaultValue);
    }

    /// <summary>
    /// Returns the value if the Maybe has a value, otherwise returns the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="maybe"></param>
    /// <param name="selector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public static async Task<K> GetValueOrDefaultAsync<T, K>(this Maybe<T> maybe, Func<T, Task<K>> selector,
        Func<Task<K>> defaultValue, CancellationToken cancellationToken = default)
    {
        if (maybe.HasNoValue)
            return await defaultValue().WithCancellation(cancellationToken);

        return await selector(maybe.Value).WithCancellation(cancellationToken);
    }

    public static async Task<T> GetValueOrDefaultAsync<T>(this Task<Maybe<T>> maybeTask, Func<Task<T>> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.GetValueOrDefaultAsync(defaultValue, cancellationToken);
    }

    public static async Task<K> GetValueOrDefaultAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, Task<K>> selector,
        K defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.GetValueOrDefaultAsync(selector, defaultValue, cancellationToken);
    }

    public static async Task<K> GetValueOrDefaultAsync<T, K>(this Task<Maybe<T>> maybeTask, Func<T, Task<K>> selector,
        Func<Task<K>> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.GetValueOrDefaultAsync(selector, defaultValue, cancellationToken);
    }

    public static async ValueTask<T> GetValueOrDefaultAsync<T>(this ValueTask<Maybe<T>> maybeTask, Func<T> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(defaultValue);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this ValueTask<Maybe<T>> maybeTask, Func<T, K> selector,
        K defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(selector, defaultValue);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this ValueTask<Maybe<T>> maybeTask, Func<T, K> selector,
        Func<K> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return maybe.GetValueOrDefault(selector, defaultValue);
    }

    public static async ValueTask<T> GetValueOrDefaultAsync<T>(this Maybe<T> maybe, Func<ValueTask<T>> valueTask, CancellationToken cancellationToken = default)
    {
        if (maybe.HasNoValue)
            return await valueTask().WithCancellation(cancellationToken);

        return maybe.Value;
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this Maybe<T> maybe, Func<T, K> selector,
        Func<ValueTask<K>> valueTask, CancellationToken cancellationToken = default)
    {
        if (maybe.HasNoValue)
            return await valueTask().WithCancellation(cancellationToken);

        return selector(maybe.Value);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this Maybe<T> maybe, Func<T, ValueTask<K>> valueTask,
        K defaultValue, CancellationToken cancellationToken = default)
    {
        if (maybe.HasNoValue)
            return defaultValue;

        return await valueTask(maybe.Value).WithCancellation(cancellationToken);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this Maybe<T> maybe, Func<T, ValueTask<K>> valueTask,
        Func<ValueTask<K>> defaultValue, CancellationToken cancellationToken = default)
    {
        if (maybe.HasNoValue)
            return await defaultValue().WithCancellation(cancellationToken);

        return await valueTask(maybe.Value).WithCancellation(cancellationToken);
    }

    public static async ValueTask<T> GetValueOrDefaultAsync<T>(this ValueTask<Maybe<T>> maybeTask, Func<ValueTask<T>> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask;
        return await maybe.GetValueOrDefaultAsync(defaultValue, cancellationToken);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this ValueTask<Maybe<T>> maybeTask, Func<T, ValueTask<K>> selector,
        K defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.GetValueOrDefaultAsync(selector, defaultValue, cancellationToken);
    }

    public static async ValueTask<K> GetValueOrDefaultAsync<T, K>(this ValueTask<Maybe<T>> maybeTask, Func<T, ValueTask<K>> selector,
        Func<ValueTask<K>> defaultValue, CancellationToken cancellationToken = default)
    {
        var maybe = await maybeTask.WithCancellation(cancellationToken);
        return await maybe.GetValueOrDefaultAsync(selector, defaultValue, cancellationToken);
    }

    public static T GetValueOrDefault<T>(in this Maybe<T> maybe, Func<T> defaultValue)
    {
        if (maybe.HasNoValue)
            return defaultValue();

        return maybe.Value;
    }

    public static K GetValueOrDefault<T, K>(in this Maybe<T> maybe, Func<T, K> selector, K defaultValue)
    {
        if (maybe.HasNoValue)
            return defaultValue;

        return selector(maybe.Value);
    }

    public static K GetValueOrDefault<T, K>(in this Maybe<T> maybe, Func<T, K> selector, Func<K> defaultValue)
    {
        if (maybe.HasNoValue)
            return defaultValue();

        return selector(maybe.Value);
    }

}