namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// Consider implementing <c>IClosedReadOnlyNode</c> if TNode is a reference type.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface IReadOnlyNode<out TNode> where TNode : IReadOnlyNode<TNode>
{
    string DisplayName => ToString() ?? "";

    /// <summary>
    /// Indicates if this type support a parent. False by default. Ignore this property if parents are ignored.
    /// If this node supports a parent, then its parent and children also supports a parent.
    /// Do throw an exception if your code expects a parent. 
    /// </summary>
    bool SupportsParent { get; }

    /// <summary>
    /// Returns children of this node.
    /// </summary>
    IReadOnlyList<TNode> Children { get; }

    /// <summary>
    /// Returns the parent node, if <c>SupportsParent</c> is true.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    TNode? Parent { get; }
}
