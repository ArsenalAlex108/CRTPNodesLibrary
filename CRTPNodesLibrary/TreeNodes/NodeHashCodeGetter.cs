namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// Gets the hash code of the content of a node, but not its children.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <param name="x"></param>
/// <param name="y"></param>
public delegate int NodeHashCodeGetter<in TNode>(TNode obj) where TNode : IReadOnlyNode<TNode>;
