﻿namespace CRTPNodesLibrary.TreeNodes.Factories;

/// <summary>
/// This type exists so that external types can use this pattern to reduce duplicated code.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="T"></typeparam>
public interface ISingletonNodeFactory<TNode, T> where TNode : ISingletonNode<TNode, T>
{

    /// <summary>
    /// Requires <c>TNode</c> to have a constructor similiar to this factory method.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="children"></param>
    /// <param name="itemComparer"></param>
    /// <returns></returns>
    TNode Create(T? value, IEnumerable<TNode>? children = null, IEqualityComparer<T>? itemComparer = null);

    /// <summary>
    /// Consider the input node to be the _root and convert it an its <c>Children</c> to a new tree of <c>TNode</c>. Doesn't use input nodes' <c>Parent</c> property.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="root"></param>
    /// <param name="selector"></param>
    /// <param name="itemComparer"></param>
    /// <returns></returns>
    TNode ToSingletonNode<TInput>(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null) where TInput : IReadOnlyNode<TInput>
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var list = root.Children
            .Select(child => ToSingletonNode(child, selector));

        var result = Create(selector(root), list, itemComparer);

        foreach (var child in list)
            SetParent(child, result);

        return result;
    }

    /// <summary>
    /// A method used to mutate the <c>Parent</c> property of a child node.
    /// This method must be implemented explicitly to avoid exposing the mutating operation.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    protected void SetParent(TNode child, TNode? parent);
}
