using CRTPNodesLibrary.TreeNodes;

using System.Diagnostics.CodeAnalysis;

namespace CRTPNodesLibrary.Comparers;

/// <summary>
/// Two trees are structurally equal when they have the same "shape" and <c>NodeComparer&lt;TResult&gt;</c> returns true for each node.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class TreeStructuralEqualityComparer<T> : EqualityComparer<T> where T : IReadOnlyNode<T>
{
    public TreeStructuralEqualityComparer()
    {
        NodeComparer = (x, y) => x.Equals(y);
        NodeHashCodeGetter = x => x.GetHashCode();
    }

    public TreeStructuralEqualityComparer(NodeComparer<T> nodeComparer, NodeHashCodeGetter<T> nodeHashCodeGetter)
    {
        NodeComparer = nodeComparer ?? throw new ArgumentNullException(nameof(nodeComparer));
        NodeHashCodeGetter = nodeHashCodeGetter ?? throw new ArgumentNullException(nameof(nodeHashCodeGetter));
    }

    public NodeComparer<T> NodeComparer { get; }
    public NodeHashCodeGetter<T> NodeHashCodeGetter { get; }

    public override bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Children.Count != y.Children.Count) return false;
        if (!NodeComparer(x, y)) return false;

        using var yEnumerator = y.Children.GetEnumerator();

        foreach (var xChild in x.Children)
        {
            if (yEnumerator.MoveNext() is false) return false;
            if (!Equals(xChild, yEnumerator.Current)) return false;
        }

        return true;
    }

    public override int GetHashCode([DisallowNull] T obj)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + NodeHashCodeGetter(obj);
            foreach (var child in obj.Children)
            {
                hash = hash * 23 + GetHashCode(child);
            }
            return hash;
        }
    }
}
