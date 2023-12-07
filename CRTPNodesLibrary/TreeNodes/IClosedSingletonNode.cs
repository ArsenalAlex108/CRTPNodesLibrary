namespace CRTPNodesLibrary.TreeNodes;
public interface IClosedSingletonNode<T> : ISingletonNode<IClosedSingletonNode<T>, T>
{

}
