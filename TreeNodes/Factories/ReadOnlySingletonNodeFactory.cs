using System.Collections.Immutable;

using CRTPNodesLibrary.TreeNodes.ExtensionTypes;

namespace CRTPNodesLibrary.TreeNodes.Factories;

public sealed class ReadOnlySingletonNodeFactory<T> : ISingletonNodeFactory<ReadOnlySingletonNode<T>, T>
{
    public static ISingletonNodeFactory<ReadOnlySingletonNode<T>, T> Factory { get; } = new ReadOnlySingletonNodeFactory<T>();

    private ReadOnlySingletonNodeFactory()
    {
    }

    public ReadOnlySingletonNode<T> Create(T? value, IEnumerable<ReadOnlySingletonNode<T>>? children, IEqualityComparer<T>? itemComparer)
    {
        children ??= Enumerable.Empty<ReadOnlySingletonNode<T>>();

        return new(value, children.ToImmutableList(), itemComparer);
    }

    void ISingletonNodeFactory<ReadOnlySingletonNode<T>, T>.SetParent(ReadOnlySingletonNode<T> child, ReadOnlySingletonNode<T>? parent)
    {
        child.Parent = parent;
    }
}

