namespace CSharpEssentials;
public readonly partial record struct Result
{
    /// <summary>
    /// Matches the result by executing the corresponding function for success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">A function to execute on success.</param>
    /// <param name="onFailure">A function to execute on failure, providing an array of errors.</param>
    /// <returns>The result of the matching function.</returns>
    public T Match<T>(Func<T> onSuccess, Func<Error[], T> onFailure) =>
        IsFailure ? onFailure(Errors) : onSuccess();

    /// <summary>
    /// Asynchronously matches the result by executing the corresponding async function for success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">An async function to execute on success.</param>
    /// <param name="onFailure">An async function to execute on failure, providing an array of errors.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation that returns the result of the matching function.</returns>
    public async Task<T> MatchAsync<T>(Func<Task<T>> onSuccess, Func<Error[], Task<T>> onFailure, CancellationToken cancellationToken) =>
        IsFailure ? await onFailure(Errors).WithCancellation(cancellationToken) : await onSuccess().WithCancellation(cancellationToken);

    /// <summary>
    /// Matches the result by executing the corresponding function for success or the first encountered error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">A function to execute on success.</param>
    /// <param name="onFirstError">A function to execute on the first error encountered.</param>
    /// <returns>The result of the matching function.</returns>
    public T MatchFirst<T>(Func<T> onSuccess, Func<Error, T> onFirstError) =>
        IsFailure ? onFirstError(FirstError) : onSuccess();

    /// <summary>
    /// Matches the result by executing the corresponding function for success or the last encountered error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">A function to execute on success.</param>
    /// <param name="onLastError">A function to execute on the last error encountered.</param>
    /// <returns>The result of the matching function.</returns>
    public T MatchLast<T>(Func<T> onSuccess, Func<Error, T> onLastError) =>
        IsFailure ? onLastError(LastError) : onSuccess();

    /// <summary>
    /// Asynchronously matches the result by executing the corresponding async function for success or the first encountered error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">An async function to execute on success.</param>
    /// <param name="onFirstError">An async function to execute on the first error encountered.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation that returns the result of the matching function.</returns>
    public async Task<T> MatchFirstAsync<T>(Func<Task<T>> onSuccess, Func<Error, Task<T>> onFirstError, CancellationToken cancellationToken) =>
        IsFailure ? await onFirstError(FirstError).WithCancellation(cancellationToken) : await onSuccess().WithCancellation(cancellationToken);

    /// <summary>
    /// Asynchronously matches the result by executing the corresponding async function for success or the last encountered error.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="onSuccess">An async function to execute on success.</param>
    /// <param name="onLastError">An async function to execute on the last error encountered.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation that returns the result of the matching function.</returns>
    public async Task<T> MatchLastAsync<T>(Func<Task<T>> onSuccess, Func<Error, Task<T>> onLastError, CancellationToken cancellationToken) =>
        IsFailure ? await onLastError(LastError).WithCancellation(cancellationToken) : await onSuccess().WithCancellation(cancellationToken);
}

/// <summary>
/// Provides extension methods for Task<Result> to simplify error handling using the Match pattern.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Awaits a Task<Result> and executes Match to handle success or failure.
    /// </summary>
    public static async Task<T> Match<T>(this Task<Result> task, Func<T> onSuccess, Func<Error[], T> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return result.Match(onSuccess, onFailure);
    }

    /// <summary>
    /// Awaits a Task<Result> and executes the asynchronous Match method.
    /// </summary>
    public static async Task<T> MatchAsync<T>(this Task<Result> task, Func<Task<T>> onSuccess, Func<Error[], Task<T>> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return await result.MatchAsync(onSuccess, onFailure, cancellationToken);
    }

    /// <summary>
    /// Awaits a Task<Result> and executes MatchFirst to handle success or failure based on the first error.
    /// </summary>
    public static async Task<T> MatchFirst<T>(this Task<Result> task, Func<T> onSuccess, Func<Error, T> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return result.MatchFirst(onSuccess, onFailure);
    }

    /// <summary>
    /// Awaits a Task<Result> and executes the asynchronous MatchFirst method.
    /// </summary>
    public static async Task<T> MatchFirstAsync<T>(this Task<Result> task, Func<Task<T>> onSuccess, Func<Error, Task<T>> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return await result.MatchFirstAsync(onSuccess, onFailure, cancellationToken);
    }

    /// <summary>
    /// Awaits a Task<Result> and executes MatchLast to handle success or failure based on the last error.
    /// </summary>
    public static async Task<T> MatchLast<T>(this Task<Result> task, Func<T> onSuccess, Func<Error, T> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return result.MatchLast(onSuccess, onFailure);
    }

    /// <summary>
    /// Awaits a Task<Result> and executes the asynchronous MatchLast method.
    /// </summary>
    public static async Task<T> MatchLastAsync<T>(this Task<Result> task, Func<Task<T>> onSuccess, Func<Error, Task<T>> onFailure, CancellationToken cancellationToken)
    {
        var result = await task.WithCancellation(cancellationToken);
        return await result.MatchLastAsync(onSuccess, onFailure, cancellationToken);
    }
}
