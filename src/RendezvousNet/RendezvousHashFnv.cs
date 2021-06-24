using System;
using System.Collections.Generic;

namespace RendezvousNet
{
    /// <summary>
    ///     Implements rendezvous hashing (also known as highest random weight (HRW) hashing).
    /// </summary>
    public class RendezvousHashFnv<TKey, TNode> : RendezvousHashBase<TKey, TNode>
        where TNode : IEquatable<TNode>, IProvideNodeId where TKey : IProvideKeyValue
    {
        private readonly int seed;

        /// <summary>
        ///     Construct new instance of RendezvousHash.
        /// </summary>
        /// <param name="initialNodes">A set of nodes, that are available initially.</param>
        /// <exception cref="ArgumentException"></exception>
        public RendezvousHashFnv(IReadOnlyCollection<TNode> initialNodes)
            : base(initialNodes)
        {
            var r = new Random();
            seed = r.Next(0, 1_000_000);
        }

        /// <summary>
        ///     Calculates hash using Fowler–Noll–Vo hashing algorithm based on node Id and key value(better performance, worse
        ///     distribution).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns>Hash value</returns>
        protected override long CalculateHash(TKey key, TNode node)
        {
            var input = key.KeyValue + node.NodeId;
            if (string.IsNullOrEmpty(input)) return default;

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
}