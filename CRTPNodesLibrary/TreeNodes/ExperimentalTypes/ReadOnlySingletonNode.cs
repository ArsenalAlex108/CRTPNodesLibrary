using System.Collections;
using System.Collections.Immutable;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods;
using CRTPNodesLibrary.TreeNodes.Factories;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;

/// <summary>
/// An immutable singleton node. Use <c>ConsistentImmutableTreeNode&lt;TResult&gt;</c> if you want a tree node with consistent <c>Parent</c> and <c>Children</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ReadOnlySingletonNode<T> : SingletonNode<ReadOnlySingletonNode<T>, T>,
                                               ISingletonNode<ReadOnlySingletonNode<T>, T>,
                                               IBuildableSingletonNode<ReadOnlySingletonNode<T>, T>
{
    public ReadOnlySingletonNode(T? value,
                                 IReadOnlyList<ReadOnlySingletonNode<T>>? children = null,
                                 IEqualityComparer<T>? itemComparer = null) : base(value, itemComparer)
    {
        Children = children ?? ImmutableArray<ReadOnlySingletonNode<T>>.Empty;

        SupportsParent = false;
    }

    public ReadOnlySingletonNode(T? value,
                                 ReadOnlySingletonNode<T>? parent,
                                 IReadOnlyList<ReadOnlySingletonNode<T>>? children = null,
                                 IEqualityComparer<T>? itemComparer = null) : base(value, itemComparer)
    {
        Children = children ?? ImmutableArray<ReadOnlySingletonNode<T>>.Empty;
        Parent = parent;

        SupportsParent = true;
    }

    public IEnumerator<ReadOnlySingletonNode<T>> GetEnumerator()
    {
        return TreeNodeExtensions.IterateDefault(this).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static ISingletonNodeFactory<ReadOnlySingletonNode<T>, T> Factory => ReadOnlySingletonNode<T>.Factory;

    public IReadOnlyList<ReadOnlySingletonNode<T>> Children { get; }

    public ReadOnlySingletonNode<T>? Parent { get; internal set; }

    public override bool SupportsParent { get; }

    protected override ReadOnlySingletonNode<T> This => this;

    public static ReadOnlySingletonNode<T> Create(T? value, IEnumerable<ReadOnlySingletonNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null)
    {
        return Factory.Create(value, children, itemComparer);
    }

    public static ReadOnlySingletonNode<T> ToSingletonNode<TInput>(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null) where TInput : IReadOnlyNode<TInput>
    {
        return Factory.ToSingletonNode(root, selector, itemComparer);
    }
}
