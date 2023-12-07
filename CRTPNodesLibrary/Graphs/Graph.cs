using System.Collections.Immutable;

using CRTPNodesLibrary.TreeNodes;

namespace CRTPNodesLibrary.Graphs;
public sealed class Graph<TKey, TNode> : IEquatable<Graph<TKey, TNode>> where TNode : IReadOnlyNode<TNode>
                                          where TKey : notnull
{
    private readonly IImmutableDictionary<TKey, TNode> _nodesTable;

    public IReadOnlyDictionary<TKey, TNode> NodesTable => _nodesTable;

    public Graph(IImmutableDictionary<TKey, TNode> nodesTable)
    {
        _nodesTable = nodesTable ?? throw new ArgumentNullException(nameof(nodesTable));
    }

    public Graph(IReadOnlyDictionary<TKey, TNode> nodesTable)
    {
        _nodesTable = nodesTable switch
        {
            IImmutableDictionary<TKey, TNode> nodes => nodes,
            null => throw new ArgumentNullException(nameof(nodesTable)),
            _ => nodesTable.ToImmutableDictionary(),
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is Graph<TKey, TNode> graph && Equals(graph);
    }

    public bool Equals(Graph<TKey, TNode>? other)
    {
        return EqualityComparer<IImmutableDictionary<TKey, TNode>>.Default.Equals(_nodesTable, other._nodesTable);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_nodesTable);
    }
}
