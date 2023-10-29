using CRTPNodesLibrary.TreeNodes;

namespace CRTPNodesLibrary.TreeNodes.Factories;


public sealed class ConsistentImmutableTreeNodeFactory<T> : ISingletonNodeFactory<ConsistentImmutableTreeNode<T>, T>
{
    public static ISingletonNodeFactory<ConsistentImmutableTreeNode<T>, T> Factory { get; } = new ConsistentImmutableTreeNodeFactory<T>();

    private ConsistentImmutableTreeNodeFactory()
    {
    }

    public ConsistentImmutableTreeNode<T> Create(T? value, IEnumerable<IClosedSingletonNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null)
    {
        return new(value, children, itemComparer);
    }

    void ISingletonNodeFactory<ConsistentImmutableTreeNode<T>, T>.SetParent(ConsistentImmutableTreeNode<T> child, ConsistentImmutableTreeNode<T>? parent)
    {
        child.Parent = parent;
    }
}

