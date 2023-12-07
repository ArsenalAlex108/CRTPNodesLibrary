using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods;
public interface ITreeIterationConfiguration<TNode> where TNode : IReadOnlyNode<TNode>
{
    ITreeIterationConfiguration<TNode> DFS(Action<IDFSSubchoices<TNode>>? dfsConfiguration = null);

    ITreeIterationConfiguration<TNode> BFS(Action<IBFSCheckCycleMode<TNode>>? bfsConfiguration = null);
}
