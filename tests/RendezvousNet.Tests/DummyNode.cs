using System;

namespace RendezvousNet.Tests
{
    public class DummyNode : IEquatable<DummyNode>, IProvideNodeId
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
