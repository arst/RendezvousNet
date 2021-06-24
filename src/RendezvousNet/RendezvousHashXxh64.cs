using System;
using System.Collections.Generic;
using System.Data.HashFunction.xxHash;
using System.Text;

namespace RendezvousNet
{
    /// <summary>
    /// Implements rendezvous hashing (also known as highest random weight (HRW) hashing) with xxHash as hashing function(better distribution, worse performance).
    /// </summary>
    public class RendezvousHashXxH64<TKey, TNode> : RendezvousHashBase<TKey, TNode> where TNode : IEquatable<TNode>, IProvideNodeId where TKey: IProvideKeyValue
    {
        private readonly IxxHash xxh64;

        /// <summary>
        /// Construct new instance of RendezvousHashXxh64.
        /// </summary>
        /// <param name="initialNodes">A set of nodes, that are available initially.</param>
        public RendezvousHashXxH64(IReadOnlyCollection<TNode> initialNodes)
            :base(initialNodes)
        {
            xxh64 = xxHashFactory.Instance.Create(new xxHashConfig { HashSizeInBits = sizeof(ulong) * 8 });
        }

        // <summary>
        /// Calculates hash using xxHash hashing algorithm based on node Id and key value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns>Hash value</returns>
        protected override long CalculateHash(TKey key, TNode node)
        {
            var computeHash = xxh64.ComputeHash(Encoding.ASCII.GetBytes(key.KeyValue + node.NodeId));
            return BitConverter.ToInt64(computeHash.Hash, 0);
        }
    }
}
