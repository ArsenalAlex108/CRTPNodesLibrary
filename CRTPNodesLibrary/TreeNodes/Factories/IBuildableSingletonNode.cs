namespace CRTPNodesLibrary.TreeNodes.Factories;

public interface IBuildableSingletonNode<TNode, T> : ISingletonNode<TNode, T> where TNode : IBuildableSingletonNode<TNode, T>
{
    static abstract ISingletonNodeFactory<TNode, T> Factory { get; }

    /// <summary>
    /// Requires <c>TNode</c> to have a constructor similiar to this factory method.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="children"></param>
    /// <param name="itemComparer"></param>
    /// <returns></returns>
    static abstract TNode Create(T? value, IEnumerable<TNode>? children = null, IEqualityComparer<T>? itemComparer = null);

    /// <summary>
    /// Consider the input node to be the _root and convert it an its <c>Children</c> to a new tree of <c>TNode</c>. Doesn't use input nodes' <c>Parent</c> property.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="root"></param>
    /// <param name="selector"></param>
    /// <param name="itemComparer"></param>
    /// <returns></returns>
    static abstract TNode ToSingletonNode<TInput>(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null) where TInput : IReadOnlyNode<TInput>;
}
