namespace CSharpEssentials;

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public Result<T> Then<T>(Func<TValue, Result<T>> onSuccess)
    {
        if (IsFailure)
            return Errors.ToResult<T>();
        return onSuccess(Value);
    }

    /// <summary>
    /// Executes the given action if the result is successful.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Result<TValue> ThenDo(Action<TValue> action)
    {
        if (IsFailure)
            return Errors;
        action(Value);
        return this;
    }

    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public Result<T> Then<T>(Func<TValue, T> onSuccess)
    {
        if (IsFailure)
            return Errors.ToResult<T>();
        return onSuccess(Value).ToResult<T>();
    }

    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public async Task<Result<T>> ThenAsync<T>(Func<TValue, Task<Result<T>>> onSuccess, CancellationToken cancellationToken = default)
    {
        if (IsFailure)
            return Errors.ToResult<T>();
        return await onSuccess(Value).WithCancellation(cancellationToken);
    }

    /// <summary>
    /// Executes the given action if the result is successful.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ThenDoAsync(Func<TValue, Task> action, CancellationToken cancellationToken = default)
    {
        if (IsFailure)
            return Errors;
        await action(Value).WithCancellation(cancellationToken);
        return this;
    }

    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public async Task<Result<T>> ThenAsync<T>(Func<TValue, Task<T>> onSuccess, CancellationToken cancellationToken = default)
    {
        if (IsFailure)
            return Errors.ToResult<T>();
        var result = await onSuccess(Value).WithCancellation(cancellationToken);
        return result.ToResult();
    }
}

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public static async Task<Result<T>> Then<TValue, T>(this Task<Result<TValue>> errorOr, Func<TValue, Result<T>> onSuccess, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Then(onSuccess);
    }

    /// <summary>
    /// Executes the given action if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public static async Task<Result<T>> Then<TValue, T>(this Task<Result<TValue>> errorOr, Func<TValue, T> onSuccess, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Then(onSuccess);
    }

    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ThenDo<TValue>(this Task<Result<TValue>> errorOr, Action<TValue> action, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.ThenDo(action);
    }

    /// <summary>
    /// Executes the given function if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public static async Task<Result<T>> ThenAsync<TValue, T>(this Task<Result<TValue>> errorOr, Func<TValue, Task<Result<T>>> onSuccess, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ThenAsync(onSuccess,cancellationToken);
    }

    /// <summary>
    /// Executes the given action if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onSuccess"></param>
    /// <returns></returns>
    public static async Task<Result<T>> ThenAsync<TValue, T>(this Task<Result<TValue>> errorOr, Func<TValue, Task<T>> onSuccess, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ThenAsync(onSuccess,cancellationToken);
    }

    /// <summary>
    /// Executes the given action if the result is successful.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ThenDoAsync<TValue>(this Task<Result<TValue>> errorOr, Func<TValue, Task> action, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ThenDoAsync(action,cancellationToken);
    }
}