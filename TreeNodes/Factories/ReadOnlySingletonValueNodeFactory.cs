using System.Collections.Immutable;

namespace CRTPNodesLibrary.TreeNodes.Factories;
public sealed class ReadOnlySingletonValueNodeFactory<T> : ISingletonNodeFactory<ReadOnlySingletonValueNode<T>, T>
{
    public static ISingletonNodeFactory<ReadOnlySingletonValueNode<T>, T> Factory { get; } = new ReadOnlySingletonValueNodeFactory<T>();

    private ReadOnlySingletonValueNodeFactory()
    {
    }

    public ReadOnlySingletonValueNode<T> Create(T? value, IEnumerable<ReadOnlySingletonValueNode<T>>? children, IEqualityComparer<T>? itemComparer)
    {
        children ??= Enumerable.Empty<ReadOnlySingletonValueNode<T>>();

        return new(value, children.ToImmutableList(), itemComparer);
    }

    void ISingletonNodeFactory<ReadOnlySingletonValueNode<T>, T>.SetParent(ReadOnlySingletonValueNode<T> child, ReadOnlySingletonValueNode<T> parent)
    {
        // pass
    }
}
