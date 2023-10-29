using System.Diagnostics.CodeAnalysis;

namespace CRTPNodesLibrary.Comparers;

/// <summary>
/// <c>IEqualityComparer&lt;T&gt;</c> wrapper of <c>Enumerable.SequenceEqual()</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class IterableStructuralEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    public static IterableStructuralEqualityComparer<T> Default { get; } = new();
    private IterableStructuralEqualityComparer()
    {
        ItemComparer = EqualityComparer<T>.Default;
    }

    public IterableStructuralEqualityComparer(IEqualityComparer<T> itemComparer)
    {
        ItemComparer = itemComparer ?? throw new ArgumentNullException(nameof(itemComparer));
    }

    public IEqualityComparer<T> ItemComparer { get; }

    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        return ReferenceEquals(x, y)
               || x is not null && y is not null && x.SequenceEqual(y, ItemComparer);
    }

    public int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        return obj.Aggregate(new HashCode(), (hash, item) =>
        {
            hash.Add(item);
            return hash;
        }).ToHashCode();
    }
}
