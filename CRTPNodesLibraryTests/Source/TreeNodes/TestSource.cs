using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes;
using CRTPNodesLibrary.TreeNodes.ExtensionTypes;

namespace CRTPNodesLibraryTests.Source.TreeNodes;
public sealed class TestSource
{
    public static TestSource Instance { get; } = new TestSource();

    public ReadOnlySingletonValueNode<int> ReadOnlySingletonValueNode { get; } = ((Func<ReadOnlySingletonValueNode<int>>)(() =>
    {
        ReadOnlySingletonValueNode<int> node6 = new(6);
        ReadOnlySingletonValueNode<int> node5 = new(5);
        ReadOnlySingletonValueNode<int> node4 = new(4);
        ReadOnlySingletonValueNode<int> node3 = new(3);
        ReadOnlySingletonValueNode<int> node2 = new(2, [node5, node6]);
        ReadOnlySingletonValueNode<int> node1 = new(1, [node3, node4]);
        ReadOnlySingletonValueNode<int> node0 = new(0, [node1, node2]);

        return node0;
    }))();

    public ReadOnlySingletonNode<int> ReadOnlySingletonNode { get; } = ((Func<ReadOnlySingletonNode<int>>)(() =>
    {
        ReadOnlySingletonNode<int> node6 = new(6);
        ReadOnlySingletonNode<int> node5 = new(5);
        ReadOnlySingletonNode<int> node4 = new(4);
        ReadOnlySingletonNode<int> node3 = new(3);
        ReadOnlySingletonNode<int> node2 = new(2, (IReadOnlyList<ReadOnlySingletonNode<int>>)[node5, node6]);
        ReadOnlySingletonNode<int> node1 = new(1, (IReadOnlyList<ReadOnlySingletonNode<int>>)[node3, node4]);
        ReadOnlySingletonNode<int> node0 = new(0, (IReadOnlyList<ReadOnlySingletonNode<int>>)[node1, node2]);

        return node0;

    }))();
}
