using System;

namespace RendezvousNet.Benchmarks
{
    internal class DummyNode : IEquatable<DummyNode>, IProvideNodeId
    {
        public DummyNode()
        {
            NodeId = Guid.NewGuid().ToString();
        }

        public bool Equals(DummyNode other)
        {
            return NodeId.Equals(other?.NodeId);
        }

        public string NodeId { get; }
    }
}