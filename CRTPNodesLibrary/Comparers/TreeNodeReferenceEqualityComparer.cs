using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using CRTPNodesLibrary.TreeNodes;

namespace CRTPNodesLibrary.Comparers;
/// <summary>
/// A reference equality comparer that can also be used for value type nodes. Uses <c>ReferenceEquals</c> on the <c>Children</c> collection to avoid infinite recursion instead.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public sealed class TreeNodeReferenceEqualityComparer<TNode> : EqualityComparer<TNode> where TNode : IReadOnlyNode<TNode>
{
    private TreeNodeReferenceEqualityComparer() { }

    public static TreeNodeReferenceEqualityComparer<TNode> Comparer { get; } = new();

    public override bool Equals(TNode? x, TNode? y)
    {
        return ReferenceEquals(x, y) || x is not null && y is not null && ReferenceEquals(x.Children, y.Children);
    }

    public override int GetHashCode([DisallowNull] TNode obj)
    {
        return RuntimeHelpers.GetHashCode(obj.Children);
    }
}
