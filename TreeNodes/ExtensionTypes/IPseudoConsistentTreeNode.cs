using System.Collections.ObjectModel;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;

/// <summary>
/// Represents a mutable node of a mutable tree, which a <c>Reconstruct()</c> method to replace this node with another node, mutating the <c>Parent</c> and <c>Children</c> 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPseudoConsistentTreeNode<T> : IReadOnlyNode<IPseudoConsistentTreeNode<T>>
{
    T? Value { get; set; }

    new IPseudoConsistentTreeNode<T>? Parent { get; set; }

    new Collection<IPseudoConsistentTreeNode<T>> Children { get; }

    /// <summary>
    /// Replace this method with a new node, not mutating this node but the <c>Parent</c> and <c>Children</c>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="children"></param>
    /// <returns></returns>
    IPseudoConsistentTreeNode<T> Reconstruct(T? value, IReadOnlyList<IPseudoConsistentTreeNode<T>>? children = null);

    IReadOnlyList<IPseudoConsistentTreeNode<T>> IReadOnlyNode<IPseudoConsistentTreeNode<T>>.Children => Children;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "Trivial")]
    bool IReadOnlyNode<IPseudoConsistentTreeNode<T>>.SupportsParent => true;
}
