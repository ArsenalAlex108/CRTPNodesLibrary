namespace CRTPNodesLibrary.TreeNodes.Factories;
public interface IBuildableSingletonNode<TNode, T> : ISingletonNode<TNode, T> where TNode : IBuildableSingletonNode<TNode, T>
{
    public static abstract ISingletonNodeFactory<TNode, T> Factory { get; }
}
