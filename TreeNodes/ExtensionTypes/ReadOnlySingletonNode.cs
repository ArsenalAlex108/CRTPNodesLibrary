using System.Collections.Immutable;

using CRTPNodesLibrary.TreeNodes;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;

/// <summary>
/// An immutable singleton node. Use <c>ConsistentImmutableTreeNode&lt;T&gt;</c> if you want a tree node with consistent <c>Parent</c> and <c>Children</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ReadOnlySingletonNode<T> : SingletonNode<ReadOnlySingletonNode<T>, T>,
                                               ISingletonNode<ReadOnlySingletonNode<T>, T>,
                                               IClosedReadOnlyNode,
                                               IClosedSingletonNode<T>
{
    public ReadOnlySingletonNode(T? value,
                                 IImmutableList<ReadOnlySingletonNode<T>> children,
                                 IEqualityComparer<T>? itemComparer = null) : base(value, itemComparer)
    {
        Children = children ?? throw new ArgumentNullException(nameof(children));

        SupportsParent = false;
    }

    public ReadOnlySingletonNode(T? value,
                                 IImmutableList<ReadOnlySingletonNode<T>> children,
                                 ReadOnlySingletonNode<T>? parent,
                                 IEqualityComparer<T>? itemComparer = null) : base(value, itemComparer)
    {
        Children = children ?? throw new ArgumentNullException(nameof(children));
        Parent = parent;

        SupportsParent = true;
    }

    public IImmutableList<ReadOnlySingletonNode<T>> Children { get; }

    public ReadOnlySingletonNode<T>? Parent { get; internal set; }

    public override bool SupportsParent { get; }

    protected override ReadOnlySingletonNode<T> This => this;

    IReadOnlyList<ReadOnlySingletonNode<T>> IReadOnlyNode<ReadOnlySingletonNode<T>>.Children => Children;

    IReadOnlyList<IClosedReadOnlyNode> IReadOnlyNode<IClosedReadOnlyNode>.Children => Children;

    IReadOnlyList<IClosedSingletonNode<T>> IReadOnlyNode<IClosedSingletonNode<T>>.Children => Children;

    IClosedReadOnlyNode? IReadOnlyNode<IClosedReadOnlyNode>.Parent => Parent;

    IClosedSingletonNode<T>? IReadOnlyNode<IClosedSingletonNode<T>>.Parent => Parent;

    public IClosedReadOnlyNode AsClosedReadOnlyNode() => this;

    public IClosedSingletonNode<T> AsClosedSingletonNode() => this;
}
