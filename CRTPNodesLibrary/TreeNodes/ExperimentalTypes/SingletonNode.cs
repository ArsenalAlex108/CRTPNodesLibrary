using CRTPNodesLibrary.Comparers;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;

/// <summary>
/// An abstract class used by tree-like singleton types to reduce boilerplate. Implements tree structural equality. The <c>Children</c> and <c>Parent</c> property are intentionally omitted.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="T"></typeparam>
public abstract class SingletonNode<TNode, T> : IEquatable<TNode> where TNode : SingletonNode<TNode, T>, ISingletonNode<TNode, T>
{
    protected SingletonNode(T? value, IEqualityComparer<T>? itemComparer = null)
    {
        Value = value;
        ItemComparer = itemComparer ?? EqualityComparer<T>.Default;

        NodeComparer = new TreeStructuralEqualityComparer<TNode>(
        (x, y) =>
        {
            return ItemComparer.Equals(x.Value, y.Value);
        },
        x =>
        {
            return x.Value is not null ? ItemComparer.GetHashCode(x.Value) : 0;
        });
    }

    protected abstract TNode This { get; }

    public T? Value { get; protected set; }

    public IEqualityComparer<T> ItemComparer { get; }

    public IEqualityComparer<TNode> NodeComparer { get; }

    public virtual bool SupportsParent => true;

    public virtual string DisplayName => Value?.ToString() ?? "";

    public override bool Equals(object? obj)
    {
        return Equals(obj as TNode);
    }

    public override int GetHashCode()
    {
        return NodeComparer.GetHashCode(This);
    }

    public bool Equals(TNode? other)
    {
        return NodeComparer.Equals(This, other);
    }
}
