using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CRTPNodesLibrary.TreeNodes.ExtensionMethods.TreeNodeExtensions;
using static CRTPNodesLibrary.Iterables.EnumerableExtensions;
using CRTPNodesLibrary.Comparers;
using CRTPNodesLibraryTests.Source.TreeNodes;
using CRTPNodesLibrary.TreeNodes.ExtensionTypes;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Tests;

[TestClass]
public class ExtensionsTests
{
    private static readonly TestSource _source = TestSource.Instance;
    private static readonly IClosedSingletonNode<int> _root = _source.ReadOnlySingletonNode.AsClosedSingletonNode(default(int));
    private static readonly IEqualityComparer<IEnumerable<int>> _comparer = IterableStructuralEqualityComparer<int>.Default;

    [TestMethod]
    public void PreorderTest()
    {
        IEnumerable<int> expected = [0, 1, 3, 4, 2, 5, 6];
        var result = _root.ConfigureIteration(i => i
                                            .DFS(j => j
                                                .Preorder()))
                                                    .Select(i => i.Value);

        Assert.AreEqual(expected.AsPrintable(default(int)),
                        result.AsPrintable(default(int)),
                        _comparer);
    }

    [TestMethod]
    public void PostorderTest()
    {
        IEnumerable<int> expected = [3, 4, 1, 5, 6, 2, 0];

        var result = _root.ConfigureIteration(i => i
                                            .DFS(j => j
                                                .Postorder()))
                                                    .Select(i => i.Value);

        Assert.AreEqual(expected.AsPrintable(default(int)),
                        result.AsPrintable(default(int)),
                        _comparer);
    }

    [TestMethod]
    public void BFSTest()
    {
        IEnumerable<int> expected = [0, 1, 2, 3, 4, 5, 6];
        var result = _root.ConfigureIteration( i => i
                                            .BFS())
                                                .Select(i => i.Value);

        Assert.AreEqual(expected.AsPrintable(default(int)),
                        result.AsPrintable(default(int)),
                        _comparer);
    }
}