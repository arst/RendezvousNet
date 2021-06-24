using System;

namespace RendezvousNet.Benchmarks
{
    internal class DummyNode : IEquatable<DummyNode>, IProvideNodeId
    {
        public string NodeId { get; }

        public DummyNode()
        {
            NodeId = Guid.NewGuid().ToString();
        }

        public bool Equals(DummyNode other)
        {
            return this.NodeId.Equals(other?.NodeId);
        }
    }
}
