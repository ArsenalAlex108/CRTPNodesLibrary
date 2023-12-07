using CRTPNodesLibrary.Iterables.ReadOnlyLists;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods;

public static partial class TreeNodeExtensions
{
    private class ClosedReadOnlyNodeAdapter<TNode>(IReadOnlyNode<TNode> root) : IClosedReadOnlyNode where TNode : IReadOnlyNode<TNode>
    {
        private readonly IReadOnlyNode<TNode> _root = root ?? throw new ArgumentNullException(nameof(root));

        public string DisplayName => _root.DisplayName;

        public bool SupportsParent => _root.SupportsParent;

        public IReadOnlyList<IClosedReadOnlyNode> Children => _root.Children.Select(i => new ClosedReadOnlyNodeAdapter<TNode>(i));

        public IClosedReadOnlyNode? Parent => (_root.Parent is null)? null :  new ClosedReadOnlyNodeAdapter<TNode>(_root.Parent);
    }
}
