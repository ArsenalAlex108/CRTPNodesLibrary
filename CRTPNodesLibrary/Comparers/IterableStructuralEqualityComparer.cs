using System.Diagnostics.CodeAnalysis;

namespace CRTPNodesLibrary.Comparers;

/// <summary>
/// <c>IEqualityComparer&lt;TResult&gt;</c> wrapper of <c>Enumerable.SequenceEqual()</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class IterableStructuralEqualityComparer<T> : EqualityComparer<IEnumerable<T>>
{
    public static new IterableStructuralEqualityComparer<T> Default { get; } = new();
    private IterableStructuralEqualityComparer()
    {
        ItemComparer = EqualityComparer<T>.Default;
    }

    public IterableStructuralEqualityComparer(IEqualityComparer<T> itemComparer)
    {
        ItemComparer = itemComparer ?? throw new ArgumentNullException(nameof(itemComparer));
    }

    public IEqualityComparer<T> ItemComparer { get; }

    public override bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        return ReferenceEquals(x, y)
               || x is not null && y is not null && x.SequenceEqual(y, ItemComparer);
    }

    public override int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        return obj.Aggregate(new HashCode(), (hash, item) =>
        {
            hash.Add(item);
            return hash;
        }).ToHashCode();
    }
}
