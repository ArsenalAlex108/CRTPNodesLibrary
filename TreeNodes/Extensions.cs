using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using CRTPNodesLibrary.Comparers;

namespace CRTPNodesLibrary.TreeNodes;

public static class Extensions
{
#warning Add more deferred methods.

    private static bool TolerantCheckCycles<TNode>(this TNode root, ImmutableHashSet<TNode> visitedNodes) where TNode : IReadOnlyNode<TNode>
    {
        foreach (var child in root.Children)
        {
            if (visitedNodes.Contains(child)) return true;
            if (child.TolerantCheckCycles(visitedNodes.Add(child))) return true;
        }

        return false;
    }

    /// <summary>
    /// Check for infinite recursion in this tree using an immutable set. Is more expensive than <c>CheckCyclesRefUnique()</c>
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns false.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool CheckCyclesRefRecursionSafe<TNode>(this TNode root) where TNode : class, IReadOnlyNode<TNode>
    {
        return root.CheckCyclesRecursionSafe<TNode>(ReferenceEqualityComparer.Instance);
    }

    /// <summary>
    /// Check for infinite recursion in this tree using an immutable set. Is more expensive than <c>CheckCyclesRefUnique()</c>
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns false.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool CheckCyclesChildrenRefRecursionSafe<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        return root.CheckCyclesRecursionSafe(TreeNodeReferenceEqualityComparer<TNode>.Comparer);
    }

    /// <summary>
    /// Check for cycles in this tree using an immutable set with a comparer. Is more expensive than <c>CheckCyclesUnique()</c>
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns false.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool CheckCyclesRecursionSafe<TNode>(this TNode root, IEqualityComparer<TNode>? comparer = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        return root.TolerantCheckCycles(ImmutableHashSet<TNode>.Empty.Add(root)
                                                             .WithComparer(comparer));
    }

    private static bool CheckCycles<TNode>(this TNode root, HashSet<TNode> visitedNodes) where TNode : IReadOnlyNode<TNode>
    {
        foreach (var child in root.Children)
        {
            if (visitedNodes.Add(child) is false) return true;

            if (child.CheckCycles(visitedNodes)) return true;
        }

        return false;
    }

    /// <summary>
    /// Check if a node is traversed more than once in a tree.
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns true.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool CheckCyclesRefUnique<TNode>(this TNode root) where TNode : class, IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        return root.CheckCyclesUnique<TNode>(ReferenceEqualityComparer.Instance);
    }

    /// <summary>
    /// Check if a node is traversed more than once in a tree.
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns true.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool CheckCyclesChildrenRefUnique<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        return root.CheckCyclesUnique(TreeNodeReferenceEqualityComparer<TNode>.Comparer);
    }

    /// <summary>
    /// Check if a node of some value appears more than once in a tree.
    /// <code>
    /// ┌─►o──┐
    /// │     ▼
    /// o     o returns true.
    /// │     ▲
    /// └─►o──┘
    /// </code>
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool CheckCyclesUnique<TNode>(this TNode root, IEqualityComparer<TNode>? comparer = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        var visitedNodes = new HashSet<TNode>(comparer)
        {
            root
        };

        return root.CheckCycles(visitedNodes);
    }

    public static string PrintTree<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        StringBuilder buffer = new();

        root.PrintTree(buffer);

        return buffer.ToString();
    }

    public static void PrintTree<TNode>(this TNode root, StringBuilder buffer) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

        root.Print(buffer, "", "");
    }

    private static void Print<TNode>(this TNode root, StringBuilder sb, string padding, string pointer) where TNode : IReadOnlyNode<TNode>
    {
        if (root != null)
        {
            _ = sb.AppendLine(padding + pointer + root.DisplayName);
            padding += pointer == "└── " ? "    " : "|   ";

            for (var i = 0; i < root.Children.Count; i++)
            {
                Print(root.Children[i], sb, padding, i == root.Children.Count - 1 ? "└── " : "├── ");
            }
        }
    }

    /// <summary>
    /// Deferredly creates a <c>preorder</c> <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering. Is not protected against infinite recursion.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="childrenCombiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakePreorderDFSTreeIterableDeferredly<TNode>(this TNode root, Func<IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? childrenCombiner = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        childrenCombiner ??= PreorderDFSChildrenCombine;

        return Defer(root);

        IEnumerable<TNode> Defer(TNode rootNode)
        {
            yield return rootNode;

            var children = childrenCombiner(root.Children.Select(node => Defer(node)));

            foreach (var node in children)
            {
                yield return node;
            }
        }
    }

    // <summary>
    /// Default implementation of DFS childrenCombiner is <c>deferred</c> preorder iteration. Input is assumed not to be <c>null</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> PreorderDFSChildrenCombine<TNode>([DisallowNull] IEnumerable<IEnumerable<TNode>> childrenEnumerations)
        where TNode : IReadOnlyNode<TNode>
    {
        foreach (var child in childrenEnumerations.SelectMany(childEnumerations => childEnumerations))
        {
            yield return child;
        }
    }

    /// <summary>
    /// Default implementation of DFS combiner is <c>deferred</c> preorder iteration. Input is assumed not to be <c>null</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="childrenEnumerations"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> PreorderDFSCombine<TNode>([DisallowNull] TNode root, [DisallowNull] IEnumerable<IEnumerable<TNode>> childrenEnumerations)
        where TNode : IReadOnlyNode<TNode>
    {
        yield return root;

        foreach (var node in PreorderDFSChildrenCombine(childrenEnumerations))
        {
            yield return node;
        }
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). Is not protected against infinite recursion.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSTreeIterable<TNode>(this TNode root, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        combiner ??= PreorderDFSCombine;

        return root.UnvalidatedMakeDFSTreeIterable(combiner);
    }

    private static IEnumerable<TNode> UnvalidatedMakeDFSTreeIterable<TNode>(this TNode root, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>> combiner) where TNode : IReadOnlyNode<TNode>
    {
        var childrenEnumerable = root.Children
            .Select(n => n.UnvalidatedMakeDFSTreeIterable(combiner)).ToList();

        return combiner(root, childrenEnumerable);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). Is protected against infinite recursion by checking <c>Children</c> collections references.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSChildrenRefRecursionSafeGraphIterable<TNode>(this TNode root, out bool recursionOccured, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : IReadOnlyNode<TNode>
    {
        return root.MakeDFSRecursionSafeGraphIterable(out recursionOccured, TreeNodeReferenceEqualityComparer<TNode>.Comparer, combiner);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). Is protected against infinite recursion by checking node references.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSRefRecursionSafeGraphIterable<TNode>(this TNode root, out bool recursionOccured, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : class, IReadOnlyNode<TNode>
    {
        return root.MakeDFSRecursionSafeGraphIterable(out recursionOccured, ReferenceEqualityComparer.Instance, combiner);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). May protect against infinite recursion if the given comparer is implemented correctly.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSRecursionSafeGraphIterable<TNode>(this TNode root, out bool recursionOccured, IEqualityComparer<TNode>? comparer = null, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        combiner ??= PreorderDFSCombine;

        return root.UnvalidatedMakeDFSTolerantGraphIterable(out recursionOccured, combiner,
                                                    ImmutableHashSet<TNode>.Empty.WithComparer(comparer));
    }

    private static IEnumerable<TNode> UnvalidatedMakeDFSTolerantGraphIterable<TNode>(this TNode root, out bool recursionOccured, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>> parentChildrenIterationCombiner, ImmutableHashSet<TNode> visitedNodes) where TNode : IReadOnlyNode<TNode>
    {
        var childrenEnumerable = new List<IEnumerable<TNode>>();

        recursionOccured = false;

        foreach (var node in root.Children)
        {
            if (visitedNodes.Contains(node))
            {
                recursionOccured = true;
                continue;
            }

            visitedNodes = visitedNodes.Add(node);
            childrenEnumerable.Add(node.UnvalidatedMakeDFSTolerantGraphIterable(out recursionOccured, parentChildrenIterationCombiner, visitedNodes));
        }

        return parentChildrenIterationCombiner(root, childrenEnumerable);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). Is protected against infinite recursion by checking <c>Children</c> collections references, so a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSChildrenRefUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : IReadOnlyNode<TNode>
    {
        return root.MakeDFSUniqueGraphIterable(out sameNodeVisited, TreeNodeReferenceEqualityComparer<TNode>.Comparer, combiner);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). Is protected against infinite recursion by checking node references, so a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSRefUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : class, IReadOnlyNode<TNode>
    {
        return root.MakeDFSUniqueGraphIterable(out sameNodeVisited, ReferenceEqualityComparer.Instance, combiner);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given childrenCombiner to decide iteration ordering (default is preorder traversal). May protect against infinite recursion if the given comparer is implemented correctly, so that a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeDFSUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, IEqualityComparer<TNode>? comparer = null, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>>? combiner = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        combiner ??= PreorderDFSCombine;

        return root.UnvalidatedMakeDFSGraphIterable(out sameNodeVisited, combiner,
                                                   new(comparer));
    }

    private static IEnumerable<TNode> UnvalidatedMakeDFSGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, Func<TNode, IEnumerable<IEnumerable<TNode>>, IEnumerable<TNode>> parentChildrenIterationCombiner, HashSet<TNode> visitedNodes) where TNode : IReadOnlyNode<TNode>
    {
        var childrenEnumerable = new List<IEnumerable<TNode>>();

        sameNodeVisited = false;

        foreach (var node in root.Children)
        {
            if (visitedNodes.Add(node) is false)
            {
                sameNodeVisited = true;
                continue;
            }

            childrenEnumerable.Add(node.UnvalidatedMakeDFSGraphIterable(out sameNodeVisited,
                                                                        parentChildrenIterationCombiner,
                                                                        visitedNodes));
        }

        return parentChildrenIterationCombiner(root, childrenEnumerable);
    }

    /// <summary>
    /// <c>Deferredly</c> creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given orderer to decide ordering of children in the queue. Is not protected protect against infinite recursion.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeBFSTreeIterableDeferredly<TNode>(this TNode root, Func<IEnumerable<TNode>, IEnumerable<TNode>>? childrenReorderer) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        return Defer();

        IEnumerable<TNode> Defer()
        {
            Queue<TNode> queue = new();

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                childrenReorderer ??= (x => x);

                foreach (var child in childrenReorderer(node.Children))
                    queue.Enqueue(child);

                yield return node;
            }
        }
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given orderer to decide ordering of children in the queue. Is protected against infinite recursion by checking <c>Children</c> collections references, so a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeBFSChildrenRefUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, Func<IEnumerable<TNode>, IEnumerable<TNode>>? childrenReorderer = null) where TNode : IReadOnlyNode<TNode>
    {
        return root.MakeBFSUniqueGraphIterable(out sameNodeVisited, TreeNodeReferenceEqualityComparer<TNode>.Comparer, childrenReorderer);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given orderer to decide ordering of children in the queue. Is protected against infinite recursion by checking node references, so a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeBFSRefUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, Func<IEnumerable<TNode>, IEnumerable<TNode>>? childrenReorderer = null) where TNode : class, IReadOnlyNode<TNode>
    {
        return root.MakeBFSUniqueGraphIterable(out sameNodeVisited, ReferenceEqualityComparer.Instance, childrenReorderer);
    }

    /// <summary>
    /// Creates an <c>Iterable&lt;TNode&gt;</c> from the given root, using the given orderer to decide ordering of children in the queue. May protect against infinite recursion if the given comparer is implemented correctly, so that a node only appears once in <c>Iterable&lt;TNode&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="combiner"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> MakeBFSUniqueGraphIterable<TNode>(this TNode root, out bool sameNodeVisited, IEqualityComparer<TNode>? comparer = null, Func<IEnumerable<TNode>, IEnumerable<TNode>>? childrenReorderer = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        sameNodeVisited = false;

        var queue = new Queue<TNode>();
        var visitedNodes = new HashSet<TNode>(comparer);

        queue.Enqueue(root);

        var result = new LinkedList<TNode>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            childrenReorderer ??= (x => x);

            foreach (var child in childrenReorderer(node.Children))
            {
                if (visitedNodes.Add(child) is false)
                {
                    sameNodeVisited = true;
                    continue;
                }

                queue.Enqueue(child);
            }

            _ = result.AddLast(node);
        }

        return result;
    }

    public static ReadOnlySingletonValueNode<string> ToReadOnlySingletonStringValueNode<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        var list = ImmutableArray<ReadOnlySingletonValueNode<string>>.Empty;

        list = list.AddRange(root.Children.Select(node => node.ToReadOnlySingletonStringValueNode()));

        return new(root.DisplayName, list);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void ThrowIfParentsNotSupported<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));

        if (!root.SupportsParent) throw new ArgumentException("Node expected to supports parent.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="node"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns></returns>
    public static TNode GetRoot<TNode>(this TNode node) where TNode : IReadOnlyNode<TNode>
    {
        node.ThrowIfParentsNotSupported();

        var root = node;

        while (root.Parent is not null) root = root.Parent;

        return root;
    }
}
