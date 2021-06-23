using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RendezvousNet.Tests
{
    public class RendezvousHashTests
    {
        [Fact]
        public void RendezvousHash_DistributesKeys_Evenly()
        {
            const int numberOfNodes = 100;
            const int numberOfKeys = 1_000_0;
            var (_, distributedKeys, _, _) = DistributeKeys(numberOfNodes, numberOfKeys);

            var distributions = distributedKeys.GroupBy(dKey => dKey.Node).Select(g => ((float)g.Count() / numberOfKeys) * 100);

            foreach (var distribution in distributions)
            {
                distribution.Should().BeInRange(0.6f, 1.4f);
            }
        }

        [Fact]
        public void RendezvousHash_Redistributes_OnlyNKeys_When_NewNodeIsAdded()
        {
            const int numberOfNodes = 10;
            const int numberOfKeys = 1_000_0;
            var (hasher, distributedKeys, originalKeys, _) = DistributeKeys(numberOfNodes, numberOfKeys);

            hasher.Add(new DummyNode());

            var newlyDistributedKeys = originalKeys.Select(key => (Key: key, Node: hasher.Get(key))).ToList();

            var oldNodeByKeyDistribution = distributedKeys.ToDictionary(k => k.Key, k => k.Node);
            var newNodeByKeyDistribution = newlyDistributedKeys.ToDictionary(k => k.Key, k => k.Node);

            var result = 0;

            foreach (var key in originalKeys)
            {
                if (oldNodeByKeyDistribution[key] != newNodeByKeyDistribution[key])
                {
                    result++;
                }
            }

            result.Should().BeInRange((numberOfKeys/(numberOfNodes + 1)) - 200, (numberOfKeys / (numberOfNodes + 1)) + 200);
        }

        [Fact]
        public void RendezvousHash_Redistributes_OnlyNKeys_When_NodeIsRemoved()
        {
            const int numberOfNodes = 10;
            const int numberOfKeys = 1_000_0;
            var (hasher, distributedKeys, originalKeys, nodes) = DistributeKeys(numberOfNodes, numberOfKeys);

            hasher.Remove(nodes.First());

            var newlyDistributedKeys = originalKeys.Select(key => (Key: key, Node: hasher.Get(key))).ToList();

            var oldNodeByKeyDistribution = distributedKeys.ToDictionary(k => k.Key, k => k.Node);
            var newNodeByKeyDistribution = newlyDistributedKeys.ToDictionary(k => k.Key, k => k.Node);

            var result = 0;

            foreach (var key in originalKeys)
            {
                if (oldNodeByKeyDistribution[key] != newNodeByKeyDistribution[key])
                {
                    result++;
                }
            }

            result.Should().BeInRange((numberOfKeys / (numberOfNodes)) - 200, (numberOfKeys / (numberOfNodes)) + 200);
        }

        private static (RendezvousHash<string, DummyNode> Hasher, List<(string Key, DummyNode Node)> DistributedKeys, List<string> originalKeys, List<DummyNode>) DistributeKeys(int numberOfNodes, int numberOfKeys)
        {
            var nodes = Enumerable.Range(0, numberOfNodes).Select(i => new DummyNode()).ToList();


            var keys = Enumerable.Range(0, numberOfKeys).Select(i => Guid.NewGuid().ToString()).ToList();

            var r = new Random();
            var fowlerNollVoSeed = r.Next(0, 1_000_000);

            var hasher = new RendezvousHash<string, DummyNode>((k, n) => FowlerNollVo(k + n.NodeId, fowlerNollVoSeed), nodes);

            var distributedKeys = keys.Select(key => (Key: key, Node: hasher.Get(key))).ToList();
            return (hasher, distributedKeys, keys, nodes);

            long FowlerNollVo(string input, int seed)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return default;
                }

                const uint h = 0x811C9DC5;
                const int p = 0x01000193;

                var z = h ^ seed;

                foreach (var symbol in input)
                {
                    z ^= symbol;
                    z = (z * p) & 0xFFFFFFF;
                }

                return z;
            }
        }

        private class DummyNode : IEquatable<DummyNode>
        {
            public Guid NodeId { get; }

            public DummyNode()
            {
                NodeId = Guid.NewGuid();
            }

            public bool Equals(DummyNode other)
            {
                return this.NodeId.Equals(other?.NodeId);
            }
        }

    }
}
