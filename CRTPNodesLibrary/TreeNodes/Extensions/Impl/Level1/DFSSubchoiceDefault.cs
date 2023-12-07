using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1.Level2;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1;
using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Impl.Level1;
public sealed class DFSSubchoiceDefault<TNode> : IDFSSubchoices<TNode> where TNode : IReadOnlyNode<TNode>
{
    public DFSCheckCycleModeDefault<TNode> CheckCycleMode { get; } = new();
    public bool IsPreorder { get; private set; } = true;


    public IDFSSubchoices<TNode> ChecksCycles(Action<IDFSCheckCycleMode<TNode>>? selection)
    {
        selection ??= i => { };
        
        selection(CheckCycleMode);

        return this;
    }

    public IDFSSubchoices<TNode> Postorder()
    {
        IsPreorder = false;

        return this;
    }

    public IDFSSubchoices<TNode> Preorder()
    {
        IsPreorder = true;

        return this;
    }
}
