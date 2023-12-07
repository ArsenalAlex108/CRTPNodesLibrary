using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;
public interface IBFSCheckCycleMode<TNode> where TNode : IReadOnlyNode<TNode>
{
    /// <summary>
    /// No cycle checking mechanism is used (most performant).
    /// </summary>
    /// <returns></returns>
    IBFSCheckCycleMode<TNode> None();

    /// <summary>
    /// Omits repeated elements using a comparer (less performant). A hashset is used during iteration.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IBFSCheckCycleMode<TNode> Unique(IEqualityComparer<TNode>? comparer = null);

    /// <summary>
    /// Omits repeated elements using a comparer (more performant). A hashset is used during iteration.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    IBFSCheckCycleMode<TNode> Unique(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection);
}
