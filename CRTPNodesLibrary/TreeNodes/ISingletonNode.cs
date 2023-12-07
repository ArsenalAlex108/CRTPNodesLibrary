namespace CRTPNodesLibrary.TreeNodes;

/// <summary>
/// Consider implementing <c>IClosedSingletonNode&lt;TResult&gt; </c> and <c>IClosedReadOnlyNode</c> if TNode is a reference type.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="T"></typeparam>
public interface ISingletonNode<out TNode, T> : IReadOnlyNode<TNode> where TNode : ISingletonNode<TNode, T>
{
    T? Value { get; }
}

