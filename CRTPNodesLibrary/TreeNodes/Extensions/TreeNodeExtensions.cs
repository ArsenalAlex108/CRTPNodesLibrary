using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using CRTPNodesLibrary.Comparers;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl;
using CRTPNodesLibrary.TreeNodes.ExtensionTypes;
using CRTPNodesLibrary.Iterables.ReadOnlyLists;
using FakeItEasy;
using System.Linq.Expressions;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods;

public static partial class TreeNodeExtensions
{
    public enum RecursionCheckOption
    {
        None,
        RecursionSafe,
        Unique
    }

    [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "<Pending>")]
    public static bool CheckCyclesEXP<TNode>(this TNode root, RecursionCheckOption recursionCheckOption, Func<RefCheckers<TNode>, IEqualityComparer<TNode>> getComparer) where TNode : IReadOnlyNode<TNode>
    {
        getComparer ??= _ => EqualityComparer<TNode>.Default;

        switch (recursionCheckOption)
        {
            case RecursionCheckOption.RecursionSafe:
                return root.CheckCyclesRecursionSafe(getComparer(RefCheckers<TNode>.Instance));
            case RecursionCheckOption.Unique:
                return root.CheckCyclesUnique(getComparer(RefCheckers<TNode>.Instance)); ;
            default:
                return false;
        }
    }

    /// <summary>
    /// Default implementation for iterating trees using DFS or BFS.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public static IEnumerable<TNode> IterateDefault<TNode>(this TNode root, Action<ITreeIterationConfiguration<TNode>>? configuration = null) where TNode : IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root);

        var configurationObj = new TreeIterationConfigurationDefault<TNode>();

        configuration ??= i => { };

        configuration(configurationObj);

        if (configurationObj.IsDFS)
            return IterateDFS(root,
                         configurationObj.DFSSubchoice.CheckCycleMode.NodeSetFactory(),
                         configurationObj.DFSSubchoice.IsPreorder);
        else
            return IterateBFS(root, configurationObj.BFSCheckCycleMode.NodeSetFactory());
    }

    /// <summary>
    /// Decorates a root to use a different iteration algorithm.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="root"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static TNode ConfigureIteration<TNode>(this TNode root, Action<ITreeIterationConfiguration<TNode>>? configuration = null) where TNode : class, IReadOnlyNode<TNode>
    {
        ArgumentNullException.ThrowIfNull(root);

        var result = A.Fake<TNode>(i => i.Wrapping(root));

        _ = A.CallTo(() => result.GetEnumerator()).ReturnsLazily(() => IterateDefault(root, configuration).GetEnumerator());

        return result;
    }

    public static IClosedReadOnlyNode AsClosedReadOnlyNode<TNode>(this TNode root) where TNode : IReadOnlyNode<TNode>
    {
        return new ClosedReadOnlyNodeAdapter<TNode>(root);
    }

    public static IClosedSingletonNode<T> AsClosedSingletonNode<TNode, T>(this TNode root) where TNode : ISingletonNode<TNode, T>
    {
        return new ClosedSingletonNodeAdapter<TNode, T>(root);
    }

    public static IClosedSingletonNode<T> AsClosedSingletonNode<TNode, T>(this TNode root, T? _0 = default) where TNode : ISingletonNode<TNode, T>
    {
        return new ClosedSingletonNodeAdapter<TNode, T>(root);
    }

    private static IEnumerable<TNode> IterateDFS<TNode>(TNode root, IImmutableSet<TNode> nodeSet, bool preorder = true) where TNode : IReadOnlyNode<TNode>
    {
        if (nodeSet.Contains(root)) yield break;

        nodeSet = nodeSet.Add(root);

        if (preorder) yield return root;

        foreach (var node in root.Children)
        {
            foreach (var nodeChild in IterateDFS(node, nodeSet, preorder))
            {
                yield return nodeChild;
            }
        }

        if (!preorder) yield return root;
    }

    private static IEnumerable<TNode> IterateBFS<TNode>(TNode root, IImmutableSet<TNode> nodeSet) where TNode : IReadOnlyNode<TNode>
    {
        var queue = new Queue<TNode>();

        queue.Enqueue(root);

        nodeSet = nodeSet.Add(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            yield return node;

            foreach (var child in node.Children)
            {
                if (nodeSet.Contains(child)) continue;
                queue.Enqueue(child);
                nodeSet = nodeSet.Add(child);
            }
        }
    }

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
    /// Get the root of a tree.
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
