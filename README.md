# CRTPNodesLibrary
Immutable Types in this library implements IEquatable<T> and override GetHashCode().
Each CRTP Node type has a non-CRTP version extending it.
Some singleton types have factories to convert one node type to its type.

All trees now implement IEnumerable<TNode>.
Added IterateDefault() to iterate a tree differently and ConfigureIteration() to decorate a tree with different iteration.
Unit tests have been added to CRTPNodesLibraryTest.
They current contain usage of Preorder, Postorder and BFS.
