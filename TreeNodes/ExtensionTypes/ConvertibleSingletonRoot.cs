using CRTPNodesLibrary.TreeNodes.Factories;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;



/// <summary>
/// A wrapper root that can be converted <c>To()</c><c>&lt;TNode&gt;</c> of other type if <c>TNode</c> implements <c>IBuildableSingletonNode&lt;TNode, T&gt;</c>.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="T"></typeparam>
public readonly struct ConvertibleSingletonRoot<TInput, T> : IReadOnlyNode<TInput>, IEquatable<ConvertibleSingletonRoot<TInput, T>> where TInput : IReadOnlyNode<TInput>
{
    public TInput Root { get; }
    public Func<TInput, T> Selector { get; }
    public IEqualityComparer<T>? ItemComparer { get; }

    public bool SupportsParent => Root.SupportsParent;

    public IReadOnlyList<TInput> Children => Root.Children;

    public TInput? Parent => Root.Parent;

    public ConvertibleSingletonRoot(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null)
    {
        Root = root;
        Selector = selector;
        ItemComparer = itemComparer;
    }

    /// <summary>
    /// Converts to <c>TNode</c> if <c>TNode</c> implements <c>IBuildableSingletonNode&lt;TNode, T&gt;</c>.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <returns></returns>
    public TNode To<TNode>() where TNode : IBuildableSingletonNode<TNode, T> => TNode.Factory.ToSingletonNode(Root, Selector, ItemComparer);

    public override bool Equals(object? obj)
    {
        return obj is ConvertibleSingletonRoot<TInput, T> selector && Equals(selector);
    }

    public bool Equals(ConvertibleSingletonRoot<TInput, T> other)
    {
        return EqualityComparer<TInput>.Default.Equals(Root, other.Root) &&
                EqualityComparer<Func<TInput, T>>.Default.Equals(Selector, other.Selector) &&
                EqualityComparer<IEqualityComparer<T>?>.Default.Equals(ItemComparer, other.ItemComparer);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Root, Selector, ItemComparer);
    }

    public static bool operator ==(ConvertibleSingletonRoot<TInput, T> left, ConvertibleSingletonRoot<TInput, T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ConvertibleSingletonRoot<TInput, T> left, ConvertibleSingletonRoot<TInput, T> right)
    {
        return !(left == right);
    }
}

