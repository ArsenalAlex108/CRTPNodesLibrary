using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

using FakeItEasy;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1.Level2;
public sealed class BFSCheckCycleModeDefault<TNode> : IBFSCheckCycleMode<TNode> where TNode : IReadOnlyNode<TNode>
{
    private static readonly IImmutableSet<TNode> FakeSet = A.Fake<IImmutableSet<TNode>>();

    public Func<IImmutableSet<TNode>> NodeSetFactory { get; set; } = () => FakeSet;

    public IBFSCheckCycleMode<TNode> None()
    {
        NodeSetFactory = () => FakeSet;

        return this;
    }

    public IBFSCheckCycleMode<TNode> Unique(IEqualityComparer<TNode>? comparer = null)
    {
        NodeSetFactory = () => new MutableImmutableHashSet<TNode>(comparer);

        return this;
    }

    public IBFSCheckCycleMode<TNode> Unique(Func<RefCheckers<TNode>, IEqualityComparer<TNode>>? selection)
    {
        if (selection is null) return Unique();

        NodeSetFactory = () => new MutableImmutableHashSet<TNode>(selection(RefCheckers<TNode>.Instance));

        return this;
    }
}
