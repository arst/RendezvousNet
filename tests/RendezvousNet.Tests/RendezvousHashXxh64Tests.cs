using System.Collections.Generic;

namespace RendezvousNet.Tests
{
    public class RendezvousHashXxH64Tests : RendezvousHashBaseTests
    {
        protected override RendezvousHashBase<DummyKey, DummyNode> GetRendezvousHashingAlgorithm(List<DummyNode> nodes)
        {
            return new RendezvousHashXxH64<DummyKey, DummyNode>(nodes);
        }
    }
}
