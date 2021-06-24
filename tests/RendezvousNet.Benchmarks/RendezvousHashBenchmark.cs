using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace RendezvousNet.Benchmarks
{
    public class RendezvousHashBenchmark
    {
        private const int NumberOfNodes = 100;
        private const int NumberOfKeys = 1_000_0;
        private readonly RendezvousHashBase<DummyKey, DummyNode> fnvHashInstance;
        private readonly List<DummyKey> keys;

        private readonly List<DummyNode> nodes;
        private readonly RendezvousHashXxH64<DummyKey, DummyNode> xxHashInstance;

        public RendezvousHashBenchmark()
        {
            nodes = Enumerable.Range(0, NumberOfNodes).Select(i => new DummyNode()).ToList();
            keys = Enumerable.Range(0, NumberOfKeys).Select(i => new DummyKey(Guid.NewGuid().ToString())).ToList();
            var r = new Random();
            var fowlerNollVoSeed = r.Next(0, 1_000_000);
            xxHashInstance = new RendezvousHashXxH64<DummyKey, DummyNode>(nodes);
            fnvHashInstance = new RendezvousHashFnv<DummyKey, DummyNode>(nodes);
        }


        [Benchmark]
        public void FnvHash()
        {
            keys.ForEach(k => fnvHashInstance.Get(k));
        }

        [Benchmark]
        public void XxHash()
        {
            keys.ForEach(k => xxHashInstance.Get(k));
        }
    }
}