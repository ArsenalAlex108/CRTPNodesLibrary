namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// Compares the content of 2 nodes, but not their children.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <param name="x"></param>
/// <param name="y"></param>
/// <returns></returns>
public delegate bool NodeComparer<in TNode>(TNode x, TNode y) where TNode : IReadOnlyNode<TNode>;
