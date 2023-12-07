using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1.Level2;

namespace CRTPNodesLibrary.TreeNodes.ExtensionMethods.Level1;

public interface IDFSSubchoices<TNode> where TNode : IReadOnlyNode<TNode>
{
    IDFSSubchoices<TNode> Preorder();

    IDFSSubchoices<TNode> Postorder();

    IDFSSubchoices<TNode> ChecksCycles(Action<IDFSCheckCycleMode<TNode>>? selection);
}
