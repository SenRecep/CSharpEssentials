namespace CSharpEssentials;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Converts a nullable value to a Maybe monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Maybe<T> AsMaybe<T>(this T? value) => value ?? default;

    /// <summary>
    /// Converts a nullable value to a Maybe monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static async ValueTask<Maybe<T>> AsMaybeAsync<T>(this ValueTask<T?> task,CancellationToken cancellationToken=default)
    {
        var nullable = await task.WithCancellation(cancellationToken);
        return nullable.AsMaybe();
    }

    /// <summary>
    /// Converts a nullable value to a Maybe monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    public static async Task<Maybe<T>> AsMaybeAsync<T>(this Task<T?> task,CancellationToken cancellationToken=default)
        where T : class
    {
        var nullable = await task.WithCancellation(cancellationToken);
        return nullable.AsMaybe();
    }
}
