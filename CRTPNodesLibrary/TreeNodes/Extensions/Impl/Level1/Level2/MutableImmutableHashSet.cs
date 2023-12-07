using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1.Level2;
public class MutableImmutableHashSet<T> : IImmutableSet<T>
{
    private readonly HashSet<T> _set;

    public MutableImmutableHashSet(IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        _set = new HashSet<T>(comparer);
    }

    public int Count => _set.Count;

    public IImmutableSet<T> Add(T value)
    {
        _ = _set.Add(value);

        return this;
    }

    public IImmutableSet<T> Clear()
    {
        _set.Clear();

        return this;
    }

    public bool Contains(T value)
    {
        return _set.Contains(value);
    }

    public IImmutableSet<T> Except(IEnumerable<T> other)
    {
        var items = _set.Except(other);

        _set.Clear();

        foreach (var item in items) _ = _set.Add(item);

        return this;
    }

    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    public IImmutableSet<T> Intersect(IEnumerable<T> other)
    {
        var items = _set.Intersect(other);

        _set.Clear();

        foreach (var item in items) _ = _set.Add(item);

        return this;
    }

    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    public IImmutableSet<T> Remove(T value)
    {
        _ = _set.Remove(value);

        return this;
    }

    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

    public IImmutableSet<T> SymmetricExcept(IEnumerable<T> other)
    {
        _set.SymmetricExceptWith(other);

        return this;
    }

    public bool TryGetValue(T equalValue, out T actualValue)
    {
        return _set.TryGetValue(equalValue, out actualValue!);
    }

    public IImmutableSet<T> Union(IEnumerable<T> other)
    {
        var items = _set.Union(other);

        _set.Clear();

        foreach (var item in items) _ = _set.Add(item);

        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
