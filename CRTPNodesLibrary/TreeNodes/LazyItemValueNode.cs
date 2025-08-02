using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.Comparers;

namespace CRTPNodesLibrary.TreeNodes;

public readonly struct LazyItemValueNode<T>(
    T value,
    Func<T, IEnumerable<T>> generator,
    IEqualityComparer<T>? itemComparer = null)
    : ISingletonNode<LazyItemValueNode<T>, T>
{
    private readonly TreeStructuralEqualityComparer<ReadOnlySingletonValueNode<T>> _treeComparer = new((x, y) => (itemComparer ?? EqualityComparer<T>.Default).Equals(x.Value, y.Value),
                            x => x.Value?.GetHashCode() ?? 0);

    public T? Value { get; init; } = value;
    private Lazy<IReadOnlyList<LazyItemValueNode<T>>> ChildrenLazy { get; } = new(() =>
        generator(value)
        .Select(i =>
        new LazyItemValueNode<T>(i, generator, itemComparer))
        .ToArray());
    public IReadOnlyList<LazyItemValueNode<T>> Children => ChildrenLazy.Value;

    public IEqualityComparer<T> ItemComparer { get; } = itemComparer ?? EqualityComparer<T>.Default;

    public bool SupportsParent => false;

    public LazyItemValueNode<T> Parent => throw new NotSupportedException();

    private static IEnumerable<LazyItemValueNode<T>> DFS(LazyItemValueNode<T> node)
    {
        yield return node;
        foreach (var child in node)
        {
            foreach (var descendant in DFS(child))
            {
                yield return descendant;
            }
        }
    }
}
