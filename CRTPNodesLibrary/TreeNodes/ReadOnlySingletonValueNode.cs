using CRTPNodesLibrary.Comparers;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods;
using CRTPNodesLibrary.TreeNodes.Factories;

using System.Collections;
using System.Collections.Immutable;

namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// An implementation of a tree node using structs. Some features are missing due to the limitations of value type.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ReadOnlySingletonValueNode<T> : ISingletonNode<ReadOnlySingletonValueNode<T>, T>, IEquatable<ReadOnlySingletonValueNode<T>>, IBuildableSingletonNode<ReadOnlySingletonValueNode<T>, T>
{
    private readonly TreeStructuralEqualityComparer<ReadOnlySingletonValueNode<T>> _treeComparer;

    public ReadOnlySingletonValueNode(T? value = default, IReadOnlyList<ReadOnlySingletonValueNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null)
    {
        Value = value;
        Children = children ?? ImmutableArray<ReadOnlySingletonValueNode<T>>.Empty;
        ItemComparer = itemComparer ?? EqualityComparer<T>.Default;

        var self = this;

        _treeComparer = new((x, y) => self.ItemComparer.Equals(x.Value, y.Value),
                            x => x.Value?.GetHashCode() ?? 0);
    }

    public static ISingletonNodeFactory<ReadOnlySingletonValueNode<T>, T> Factory => ReadOnlySingletonValueNodeFactory<T>.Factory;

    public T? Value { get; init; }

    public IReadOnlyList<ReadOnlySingletonValueNode<T>> Children { get; }

    public IEqualityComparer<T> ItemComparer { get; }

    public readonly string DisplayName => Value?.ToString() ?? "";

    public bool SupportsParent => false;

    ReadOnlySingletonValueNode<T> IReadOnlyNode<ReadOnlySingletonValueNode<T>>.Parent => throw new NotSupportedException();

    public static ReadOnlySingletonValueNode<T> Create(T? value, IEnumerable<ReadOnlySingletonValueNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null)
    {
        return new(value, (children ?? Enumerable.Empty<ReadOnlySingletonValueNode<T>>()).ToImmutableList(), itemComparer);
    }

    public static ReadOnlySingletonValueNode<T> ToSingletonNode<TInput>(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null) where TInput : IReadOnlyNode<TInput>
    {
        return Factory.ToSingletonNode(root, selector, itemComparer);
    }

    public bool Equals(ReadOnlySingletonValueNode<T> other) => _treeComparer.Equals(this, other);

    public override bool Equals(object? obj) => obj is ReadOnlySingletonValueNode<T> node && Equals(node);

    public override int GetHashCode() => _treeComparer.GetHashCode(this);

    public static bool operator ==(ReadOnlySingletonValueNode<T> left, ReadOnlySingletonValueNode<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlySingletonValueNode<T> left, ReadOnlySingletonValueNode<T> right)
    {
        return !(left == right);
    }
}
