using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRTPNodesLibrary.Iterables.ReadOnlyLists;
public static partial class ReadOnlyListExtensions
{
    /// <summary>
    ///
    /// Summary:
    ///     Projects each element of a sequence into a new form by incorporating the element's
    ///     index.
    ///
    /// Parameters:
    ///   source:
    ///     A sequence of values to invoke a transform function on.
    ///
    ///   selector:
    ///     A transform function to apply to each source element; the second parameter of
    ///     the function represents the index of the source element.
    ///
    /// Type parameters:
    ///   TSource:
    ///     The type of the elements of source.
    ///
    ///   TResult:
    ///     The type of the value returned by selector.
    ///
    /// Returns:
    ///     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
    ///     invoking the transform function on each element of source.
    ///
    /// Exceptions:
    ///   T:System.ArgumentNullException:
    ///     source or selector is null.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IReadOnlyList<TResult> Select<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> selector)
    {
        return new ReadOnlyListAdapter<TSource, TResult>(source, selector);
    }
}
