using CRTPNodesLibrary.Iterables.ReadOnlyLists;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods;

public static partial class TreeNodeExtensions
{
    private class ClosedSingletonNodeAdapter<TNode, T>(ISingletonNode<TNode, T> root) : IClosedSingletonNode<T> where TNode : ISingletonNode<TNode, T>
    {
        private readonly ISingletonNode<TNode, T> _root = root ?? throw new ArgumentNullException(nameof(root));

        public string DisplayName => _root.DisplayName;

        public bool SupportsParent => _root.SupportsParent;

        public IReadOnlyList<IClosedSingletonNode<T>> Children => _root.Children.Select(i => new ClosedSingletonNodeAdapter<TNode, T>(i));

        public IClosedSingletonNode<T>? Parent => (_root.Parent is null) ? null : new ClosedSingletonNodeAdapter<TNode, T>(_root.Parent);

        public T? Value => _root.Value;
    }
}
