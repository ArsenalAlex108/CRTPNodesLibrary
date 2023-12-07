using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1.Level2;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl;
public sealed class TreeIterationConfigurationDefault<TNode> : ITreeIterationConfiguration<TNode> where TNode : IReadOnlyNode<TNode>
{
    public DFSSubchoiceDefault<TNode> DFSSubchoice { get; } = new();
    public BFSCheckCycleModeDefault<TNode> BFSCheckCycleMode { get; } = new();
    public bool IsDFS { get; private set; }


    public ITreeIterationConfiguration<TNode> BFS(Action<IBFSCheckCycleMode<TNode>>? bfsConfiguration = null)
    {
        IsDFS = false;

        bfsConfiguration ??= i => { };

        bfsConfiguration(BFSCheckCycleMode);

        return this;
    }

    public ITreeIterationConfiguration<TNode> DFS(Action<IDFSSubchoices<TNode>>? dfsConfiguration = null)
    {
        IsDFS = true;

        dfsConfiguration ??= i => { };

        dfsConfiguration(DFSSubchoice);

        return this;
    }
}
