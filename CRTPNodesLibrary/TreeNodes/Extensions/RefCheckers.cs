using System.Diagnostics.CodeAnalysis;

using CRTPNodesLibrary.Comparers;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods;


public sealed class RefCheckers<TNode> where TNode : IReadOnlyNode<TNode>
{
    private static readonly Lazy<RefCheckers<TNode>> _instance = new(false);

    private RefCheckers() { }

    public static RefCheckers<TNode> Instance => _instance.Value;

    public IEqualityComparer<TNode> DefaultComparer => EqualityComparer<TNode>.Default;

    public IEqualityComparer<TNode> ReferenceComparer
    {
        get
        {
            if (typeof(TNode).IsValueType) throw new NotSupportedException($"""
                                                                            Not supported for Value Type "{typeof(TNode)}".
                                                                            """);

            return (ReferenceEqualityComparer.Instance as IEqualityComparer<TNode>)!;
        }
    }

    public IEqualityComparer<TNode> ChildrenReferenceComparer => TreeNodeReferenceEqualityComparer<TNode>.Comparer;
}

