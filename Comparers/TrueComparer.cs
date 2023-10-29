using System.Diagnostics.CodeAnalysis;

namespace CRTPNodesLibrary.Comparers;

/// <summary>
/// <c>Equals()</c> always return true, <c>GetHashCode()</c> always return <c>0</c>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class TrueComparer<T> : IEqualityComparer<T>
{
    private TrueComparer() { }

    public static TrueComparer<T> Comparer { get; } = new();

    public bool Equals(T? x, T? y) => true;

    public int GetHashCode([DisallowNull] T obj) => 0;
}
