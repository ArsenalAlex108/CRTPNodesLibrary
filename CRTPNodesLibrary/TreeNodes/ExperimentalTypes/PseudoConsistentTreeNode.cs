using System.Collections;
using System.Collections.ObjectModel;

using CRTPNodesLibrary.Comparers;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods;

namespace CRTPNodesLibrary.TreeNodes.ExtensionTypes;

/// <summary>
/// This mutable type is neither thread safe nor ensured to be consistent, and should only be used in limited scopes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PseudoConsistentTreeNode<T> : IPseudoConsistentTreeNode<T>,
                                           IEquatable<IPseudoConsistentTreeNode<T>>
{
    public PseudoConsistentTreeNode(T? value, IEnumerable<IPseudoConsistentTreeNode<T>>? children = null, IEqualityComparer<T>? itemComparer = null)
    {
        Value = value;

        ItemComparer = itemComparer ?? EqualityComparer<T>.Default;

        NodeComparer = new TreeStructuralEqualityComparer<IPseudoConsistentTreeNode<T>>(
(x, y) =>
        {
            return ItemComparer.Equals(x.Value, y.Value);
        },
        x =>
        {
            return x.Value is not null ? ItemComparer.GetHashCode(x.Value) : 0;
        });

        ArgumentNullException.ThrowIfNull(children, nameof(children));

        foreach (var child in children)
        {
            child.Parent = this;

            Children.Add(child);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "<Pending>")]
    IEnumerator IEnumerable.GetEnumerator() => ((IPseudoConsistentTreeNode<T>)this).GetEnumerator();

    public T? Value { get; set; }
    public IPseudoConsistentTreeNode<T>? Parent { get; set; }

    public Collection<IPseudoConsistentTreeNode<T>> Children { get; } = [];

    public IEqualityComparer<T> ItemComparer { get; }

    public IEqualityComparer<IPseudoConsistentTreeNode<T>> NodeComparer { get; }

    public bool SupportsParent => true;

    Collection<IPseudoConsistentTreeNode<T>> IPseudoConsistentTreeNode<T>.Children => Children;

    public override bool Equals(object? obj)
    {
        return Equals(obj as IPseudoConsistentTreeNode<T>);
    }

    public override int GetHashCode()
    {
        return NodeComparer.GetHashCode(this);
    }

    public bool Equals(IPseudoConsistentTreeNode<T>? other)
    {
        return NodeComparer.Equals(this, other);
    }

    public IPseudoConsistentTreeNode<T> Reconstruct(T? value, IReadOnlyList<IPseudoConsistentTreeNode<T>>? children = null)
    {
        var result = new PseudoConsistentTreeNode<T>(Value, children ?? Children);

        if (Parent is not null)
            foreach (var index in from index in Enumerable.Range(0, Parent.Children.Count)
                                  let child = Parent.Children[index]
                                  where ReferenceEquals(this, child)
                                  select index)
            {
                Parent.Children[index] = result;
                break;
            }

        foreach (var child in Children)
        { child.Parent = this; }

        return result;
    }
}
