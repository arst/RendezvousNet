using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RendezvousNet.Benchmarks
{
    public class RendezvousHashBenchmark
    {
        private const int NumberOfNodes = 100;
        private const int NumberOfKeys = 1_000_0;

        private readonly List<DummyNode> nodes;
        private readonly List<DummyKey> keys;
        private readonly RendezvousHashXxH64<DummyKey, DummyNode> xxHashInstance;
        private readonly RendezvousHashBase<DummyKey, DummyNode> fnvHashInstance;

        public RendezvousHashBenchmark()
        {
            nodes = Enumerable.Range(0, NumberOfNodes).Select(i => new DummyNode()).ToList();
            keys = Enumerable.Range(0, NumberOfKeys).Select(i => new DummyKey(Guid.NewGuid().ToString())).ToList();
            var r = new Random();
            var fowlerNollVoSeed = r.Next(0, 1_000_000);
            xxHashInstance = new RendezvousHashXxH64<DummyKey, DummyNode>(nodes);
            fnvHashInstance = new RendezvousHashBase<DummyKey, DummyNode>((key, node) =>
            {
                var input = key.KeyValue + node.NodeId;
                if (string.IsNullOrEmpty(input))
                {
                    return default;
                }

                const uint h = 0x811C9DC5;
                const int p = 0x01000193;

                var z = h ^ fowlerNollVoSeed;

                foreach (var symbol in input)
                {
                    z ^= symbol;
                    z = (z * p) & 0xFFFFFFF;
                }

                return z;
            }, nodes);
        }


        [Benchmark]
        public void FnvHash() => keys.ForEach(k => fnvHashInstance.Get(k));

        [Benchmark]
        public void XxHash() => keys.ForEach(k => xxHashInstance.Get(k));
    }
}
