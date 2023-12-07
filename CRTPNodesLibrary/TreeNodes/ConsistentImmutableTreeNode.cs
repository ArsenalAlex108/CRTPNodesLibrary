using System.Collections;
using System.Collections.Immutable;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods;
using CRTPNodesLibrary.TreeNodes.ExtensionTypes;
using CRTPNodesLibrary.TreeNodes.Factories;

namespace CRTPNodesLibrary.TreeNodes;


public sealed class ConsistentImmutableTreeNode<T> : SingletonNode<ConsistentImmutableTreeNode<T>, T>,
                                                     ISingletonNode<ConsistentImmutableTreeNode<T>, T>,
                                                     IConsistentImmutableTreeNode<T>,
                                                     IBuildableSingletonNode<ConsistentImmutableTreeNode<T>, T>
{
    public ConsistentImmutableTreeNode(T? value, IEnumerable<IClosedSingletonNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null) : base(value, itemComparer)
    {
        var builder = ImmutableArray.CreateBuilder<ConsistentImmutableTreeNode<T>>();

        if (children is not null)
            foreach (var child in children)
            {
                ConsistentImmutableTreeNode<T> newChild = child is ConsistentImmutableTreeNode<T> concreteNode
                    ? concreteNode.BuildDownwards()
                    : new ConsistentImmutableTreeNode<T>(child.Value,
                                                                  child.Children)
                                                                    .BuildDownwards();
                newChild.Parent = this;

                builder.Add(newChild);
            }

        Children = builder.ToImmutable();
    }

    /// <summary>
    /// Reconstruct the entire a tree with this node replaced with a new node with new data.
    /// If children is null, then the old node's children is used.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="children"></param>
    /// <returns>The replaced node, and the _root of the tree in the out parameter</returns>
    public ConsistentImmutableTreeNode<T> Reconstruct(T? value,
                                                        out ConsistentImmutableTreeNode<T> root,
                                                      IReadOnlyList<IClosedSingletonNode<T>>? children = null)
    {
        root = this.GetRoot();
        var target = this;
        var replacement = new ConsistentImmutableTreeNode<T>(value,
                                                             children
                                                             ?? Children.Select(i => i.AsClosedSingletonNode(default(T))),
                                                             ItemComparer);

        root = root.BuildDownwards(target, replacement);

        return replacement;
    }

    private ConsistentImmutableTreeNode<T> BuildDownwards()
    {
        var builder = ImmutableArray.CreateBuilder<ConsistentImmutableTreeNode<T>>();

        foreach (var child in Children)
        {
            var newChild = child.BuildDownwards();
            builder.Add(newChild);
        }

        var result = new ConsistentImmutableTreeNode<T>(Value,
                                                        builder.ToImmutable()
                                                               .Select(i => i.AsClosedSingletonNode(default(T))),
                                                        ItemComparer);

        foreach (var child in result.Children)
        {
            child.Parent = result;
        }

        return result;
    }

    private ConsistentImmutableTreeNode<T> BuildDownwards(ConsistentImmutableTreeNode<T> target, ConsistentImmutableTreeNode<T> replacement)
    {
        var builder = ImmutableArray.CreateBuilder<ConsistentImmutableTreeNode<T>>();

        if (ReferenceEquals(this, target)) return replacement;

        foreach (var child in Children)
        {
            var newChild = child.BuildDownwards(target, replacement);
            builder.Add(newChild);
        }

        var result = new ConsistentImmutableTreeNode<T>(Value,
                                                        builder.ToImmutable()
                                                               .Select(i => i.AsClosedSingletonNode(default(T))),
                                                        ItemComparer);

        foreach (var child in result.Children)
        {
            child.Parent = result;
        }

        return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((ISingletonNode<ConsistentImmutableTreeNode<T>, T>)this).GetEnumerator();

    public IConsistentImmutableTreeNode<T> Reconstruct(T? value, out IConsistentImmutableTreeNode<T> root, IReadOnlyList<IClosedSingletonNode<T>>? children = null)
    {
        return Reconstruct(value, out root, children);
    }

    public static ConsistentImmutableTreeNode<T> ToSingletonNode<TInput>(TInput root, Func<TInput, T> selector, IEqualityComparer<T>? itemComparer = null) where TInput : IReadOnlyNode<TInput>
    {
        return Factory.ToSingletonNode(root, selector, itemComparer);
    }

    static ConsistentImmutableTreeNode<T> IBuildableSingletonNode<ConsistentImmutableTreeNode<T>, T>.Create(T? value, IEnumerable<ConsistentImmutableTreeNode<T>>? children, IEqualityComparer<T>? itemComparer)
    {
        return new(value, children?.Select(i => i.AsClosedSingletonNode(default(T))), itemComparer);
    }

    public ImmutableArray<ConsistentImmutableTreeNode<T>> Children { get; }

    public ConsistentImmutableTreeNode<T>? Parent { get; internal set; }

    IReadOnlyList<IConsistentImmutableTreeNode<T>> IReadOnlyNode<IConsistentImmutableTreeNode<T>>.Children => Children;

    IConsistentImmutableTreeNode<T>? IReadOnlyNode<IConsistentImmutableTreeNode<T>>.Parent => Parent;

    IReadOnlyList<ConsistentImmutableTreeNode<T>> IReadOnlyNode<ConsistentImmutableTreeNode<T>>.Children => Children;

    protected override ConsistentImmutableTreeNode<T> This => this;

    public static ISingletonNodeFactory<ConsistentImmutableTreeNode<T>, T> Factory => ConsistentImmutableTreeNodeFactory<T>.Factory;
}
