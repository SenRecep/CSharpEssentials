namespace CSharpEssentials;

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public Result<TValue> Else(Func<IReadOnlyList<Error>, Error> onFailure)
    {
        if (IsSuccess) return Value;
        return onFailure(Errors).ToResult<TValue>();
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public Result<TValue> Else(Func<IReadOnlyList<Error>, IReadOnlyList<Error>> onFailure)
    {
        if (IsSuccess) return Value;
        return onFailure(Errors).ToResult<TValue>();
    }

    /// <summary>
    /// If the operation failed, replaces the current errors with the provided error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public Result<TValue> Else(Error error)
    {
        if (IsSuccess) return Value;
        return error.ToResult<TValue>();
    }

    /// <summary>
    /// Asynchronously executes a function to generate a single error if the operation failed and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public Result<TValue> Else(Func<IReadOnlyList<Error>, TValue> onFailure)
    {
        if (IsSuccess) return Value;
        return onFailure(Errors);
    }

    /// <summary>
    /// Asynchronously executes a function to generate a single error if the operation failed and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public Result<TValue> Else(TValue onFailure)
    {
        if (IsSuccess) return Value;
        return onFailure;
    }

    /// <summary>
    /// If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ElseAsync(Func<IReadOnlyList<Error>, Task<TValue>> onFailure, CancellationToken cancellationToken = default)
    {
        if (IsSuccess) return Value;
        var result = await onFailure(Errors).WithCancellation(cancellationToken);
        return result;
    }

    /// <summary>
    /// If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ElseAsync(Func<IReadOnlyList<Error>, Task<Error>> onFailure, CancellationToken cancellationToken = default)
    {
        if (IsSuccess) return Value;
        var result = await onFailure(Errors).WithCancellation(cancellationToken);
        return result.ToResult<TValue>();
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ElseAsync(Func<IReadOnlyList<Error>, Task<IReadOnlyList<Error>>> onFailure, CancellationToken cancellationToken = default)
    {
        if (IsSuccess) return Value;
        var result = await onFailure(Errors).WithCancellation(cancellationToken);
        return result.ToResult<TValue>();
    }

    /// <summary>
    /// If the operation failed, replaces the current errors with the provided error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ElseAsync(Task<Error> error, CancellationToken cancellationToken = default)
    {
        if (IsSuccess) return Value;
        var result = await error.WithCancellation(cancellationToken);
        return result.ToResult<TValue>();
    }

    /// <summary>
    /// Asynchronously executes a function to generate a single error if the operation failed and returns a new Result with that error.
    /// </summary>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public async Task<Result<TValue>> ElseAsync(Task<TValue> onFailure, CancellationToken cancellationToken = default)
    {
        if (IsSuccess) return Value;
        var result = await onFailure.WithCancellation(cancellationToken);
        return result;
    }
}

public static partial class ResultExtensions
{
    /// <summary>
    /// If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, TValue> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Else(onFailure);
    }

    /// <summary>
    /// If the operation failed, replaces the current errors with the provided error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> errorOr, TValue onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Else(onFailure);
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, Task<TValue>> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ElseAsync(onFailure,cancellationToken);
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> errorOr, Task<TValue> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ElseAsync(onFailure,cancellationToken);
    }

    /// <summary>
    ///  If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, Error> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Else(onFailure);
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, IReadOnlyList<Error>> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Else(onFailure);
    }

    /// <summary>
    /// If the operation failed, replaces the current errors with the provided error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> Else<TValue>(this Task<Result<TValue>> errorOr, Error error, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return result.Else(error);
    }
    /// <summary>
    /// If the operation failed, executes a function to generate a single error and returns a new Result with that error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, Task<Error>> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ElseAsync(onFailure,cancellationToken);
    }

    /// <summary>
    /// If the operation failed, executes a function to generate multiple errors and returns a new Result with those errors.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> errorOr, Func<IReadOnlyList<Error>, Task<IReadOnlyList<Error>>> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ElseAsync(onFailure,cancellationToken);
    }

    /// <summary>
    /// If the operation failed, replaces the current errors with the provided error.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errorOr"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public static async Task<Result<TValue>> ElseAsync<TValue>(this Task<Result<TValue>> errorOr, Task<Error> onFailure, CancellationToken cancellationToken = default)
    {
        var result = await errorOr.WithCancellation(cancellationToken);
        return await result.ElseAsync(onFailure,cancellationToken);
    }
}