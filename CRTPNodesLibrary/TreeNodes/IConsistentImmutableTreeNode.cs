namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// An interface representing a tree node with consistent <c>Parent</c> and <c>Children</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IConsistentImmutableTreeNode<T> : ISingletonNode<IConsistentImmutableTreeNode<T>, T>
{
    /// <summary>
    /// Replace this node with a new node. The entire tree is then reconstructed to avoid affecting the old tree.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="root"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    IConsistentImmutableTreeNode<T> Reconstruct(T? value, out IConsistentImmutableTreeNode<T> root, IReadOnlyList<IClosedSingletonNode<T>>? children = null);
}