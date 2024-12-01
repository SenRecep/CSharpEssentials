using System;
using System.Runtime.CompilerServices;

namespace CSharpEssentials;

public static partial class MaybeExtensions
{
    /// <summary>
    ///  Projects the value of a Maybe into a new form if it has a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<U> Choose<T, U>(this IEnumerable<Maybe<T>> source, Func<T, U> selector)
    {
        foreach (var item in source)
            if (item.HasValue)
                yield return selector(item.Value);
    }
    /// <summary>
    /// Projects the value of a Maybe into a new form if it has a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> Choose<T>(this IEnumerable<Maybe<T>> source)
    {
        foreach (var item in source)
            if (item.HasValue)
                yield return item.Value;
    }

   /// <summary>
   /// Projects the value of a Maybe into a new form if it has a value.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="source"></param>
   /// <param name="cancellationToken"></param>
   /// <returns></returns>
    public static async IAsyncEnumerable<T> ChooseAsync<T>(this IEnumerable<Task<Maybe<T>>> source,[EnumeratorCancellation] CancellationToken cancellationToken=default)
    {
        await foreach (var task in Task.WhenEach(source).WithCancellation(cancellationToken))
        {
            var result= await task;
            if (result.HasValue)
                yield return result.Value;
        }
    }

    /// <summary>
    /// Projects the value of a Maybe into a new form if it has a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async IAsyncEnumerable<U> ChooseAsync<T, U>(this IEnumerable<Task<Maybe<T>>> source, Func<T, U> selector,[EnumeratorCancellation] CancellationToken cancellationToken=default)
    {
        await foreach (var task in Task.WhenEach(source).WithCancellation(cancellationToken))
        {
            var result= await task;
            if (result.HasValue)
                yield return selector(result.Value);
        }
    }
}
