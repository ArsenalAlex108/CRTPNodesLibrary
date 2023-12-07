namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;
public interface IDFSCheckCycleMode<TNode> where TNode : IReadOnlyNode<TNode>
{
    /// <summary>
    /// No cycle checking mechanism is used (most performant).
    /// </summary>
    /// <returns></returns>
    IDFSCheckCycleMode<TNode> None();

    /// <summary>
    /// Omits repeated elements using a comparer (less performant). A hashset is used during iteration.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IDFSCheckCycleMode<TNode> Unique(IEqualityComparer<TNode>? comparer = null);

    /// <summary>
    /// Omits repeated elements using a comparer (more performant). A hashset is used during iteration.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IDFSCheckCycleMode<TNode> Unique(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection);

    /// <summary>
    /// Protects from infinite recursion using a comparer (least performant). An immutable set is created for each node.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IDFSCheckCycleMode<TNode> InfiniteCycleSafe(IEqualityComparer<TNode>? comparer = null);

    /// <summary>
    /// Protects from infinite recursion using a comparer (least performant). An immutable set is created for each node.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IDFSCheckCycleMode<TNode> InfiniteCycleSafe(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection);
}
