using System.Collections;

namespace CRTPNodesLibrary.Iterables.ReadOnlyLists;
public static partial class ReadOnlyListExtensions
{
    private class ReadOnlyListAdapter<TSource, TResult>(IReadOnlyList<TSource> source, Func<TSource, TResult> selector) : IReadOnlyList<TResult>
    {
        private readonly IReadOnlyList<TSource> source = source ?? throw new ArgumentNullException(nameof(source));
        private readonly Func<TSource, TResult> selector = selector ?? throw new ArgumentNullException(nameof(selector));

        public TResult this[int index] => selector(source[index]);

        public int Count => source.Count;

        public IEnumerator<TResult> GetEnumerator() => Enumerable.Select(source, selector).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
