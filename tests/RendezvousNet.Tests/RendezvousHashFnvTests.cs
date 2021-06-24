using System.Collections.Generic;

namespace RendezvousNet.Tests
{
    public class RendezvousHashFnvTests : RendezvousHashBaseTests
    {
        protected override RendezvousHashBase<DummyKey, DummyNode> GetRendezvousHashingAlgorithm(List<DummyNode> nodes)
        {
            return new RendezvousHashFnv<DummyKey, DummyNode>(nodes);
        }
    }
}