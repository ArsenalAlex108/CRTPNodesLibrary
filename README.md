# CRTPNodesLibrary
Immutable Types in this library implements IEquatable<T> and override GetHashCode().
Each CRTP Node type has a non-CRTP version extending it.
Some singleton types have factories to convert one node type to its type.