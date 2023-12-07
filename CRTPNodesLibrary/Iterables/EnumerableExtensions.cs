using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.Decoration;

using FakeItEasy;

namespace CRTPNodesLibrary.Iterables;
public static partial class EnumerableExtensions
{
    /// <summary>
    /// Materialize an IEnumerable if it does not implement either the generic IReadOnlyCollection or ICollection interfaces.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<T> Materialize<T>(this IEnumerable<T> values)
    {
        return values switch
        {
            IReadOnlyCollection<T> readOnly => readOnly,
            ICollection<T> mutable => ((Func<IReadOnlyCollection<T>>)(() =>
            {
                var result = A.Fake<IReadOnlyCollection<T>>(i => i.Implements<IEnumerable>().Strict());

                _ = A.CallTo(() => result.GetEnumerator()).ReturnsLazily(mutable.GetEnumerator);
                _ = A.CallTo(() => ((IEnumerable)result).GetEnumerator()).ReturnsLazily(mutable.GetEnumerator);
                _ = A.CallTo(() => result.Count).ReturnsLazily(() => mutable.Count);

                return result;

            }))(),
            _ => values.ToList(),
        };
    }

    public static IEnumerable<int> AsEnumerable(this Range range)
    {
        var enumerator = new NonRefStructIntEnumerator(range);

        return Defer(enumerator);
    }

    private static IEnumerable<int> Defer(NonRefStructIntEnumerator enumerator)
    {
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    public static IntEnumerator GetEnumerator(this Range range)
    {
        return new IntEnumerator(range);
    }

    public static string Print<T>(this IEnumerable<T> values)
    {
        values ??= Enumerable.Empty<T>();

        var materialized = values.Materialize();

        // [ 0, 1 ]
        var buffer = new StringBuilder(materialized.Count * 3 + 4);

        _ = buffer.Append("[ ");

        foreach (var item in materialized)
        {
            _ = buffer.Append((string)$"{item}, ");
        }

        _ = buffer.Remove(buffer.Length - 2, 2);

        _ = buffer.Append(" ]");

        return buffer.ToString();
    }

    public static TEnumerable AsPrintable<T, TEnumerable>(this TEnumerable values) where TEnumerable : class, IEnumerable<T>
    {
        return Decorate.ToString(values, values.Print);
    }

    public static TEnumerable AsPrintable<T, TEnumerable>(this TEnumerable values, T? _0 = default) where TEnumerable : class, IEnumerable<T>
    {
        return Decorate.ToString(values, values.Print);
    }
}
