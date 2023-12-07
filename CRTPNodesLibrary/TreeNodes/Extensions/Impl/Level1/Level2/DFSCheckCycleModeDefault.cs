using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1.Level2;
public sealed class DFSCheckCycleModeDefault<TNode> : IDFSCheckCycleMode<TNode> where TNode : IReadOnlyNode<TNode>
{
    private readonly BFSCheckCycleModeDefault<TNode> _bfs = new();

    public Func<IImmutableSet<TNode>> NodeSetFactory => _bfs.NodeSetFactory;

    public IDFSCheckCycleMode<TNode> InfiniteCycleSafe(IEqualityComparer<TNode>? comparer = null)
    {
        _bfs.NodeSetFactory = () => ImmutableHashSet<TNode>.Empty.WithComparer(comparer);

        return this;
    }

    public IDFSCheckCycleMode<TNode> InfiniteCycleSafe(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection)
    {
        if (selection is null) return InfiniteCycleSafe();

        _bfs.NodeSetFactory = () =>
            ImmutableHashSet<TNode>.Empty.WithComparer(
                selection(RefCheckers<TNode>.Instance));

        return this;
    }

    public IDFSCheckCycleMode<TNode> None()
    {
        _ = _bfs.None();

        return this;
    }

    public IDFSCheckCycleMode<TNode> Unique(IEqualityComparer<TNode>? comparer = null)
    {
        _ = _bfs.Unique(comparer);

        return this;
    }

    public IDFSCheckCycleMode<TNode> Unique(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection)
    {
        _ = _bfs.Unique(selection);

        return this;
    }
}
