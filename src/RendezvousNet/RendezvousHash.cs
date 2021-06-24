using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RendezvousNet
{
    /// <summary>
    /// Implements rendezvous hashing (also known as highest random weight (HRW) hashing).
    /// </summary>
    public class RendezvousHash<TKey, TNode> where TNode: IEquatable<TNode>
    {
        private readonly Func<TKey, TNode, long> hashingFunction;
        private readonly ConcurrentList<TNode> nodes;

        /// <summary>
        /// Construct new instance of RendezvousHash.
        /// </summary>
        /// <param name="hashingFunction">A function to calculate a combined hash for node and key. This function must provide good uniform distribution. E.g. Fowler-Noll-Vo.</param>
        /// <param name="initialNodes">A set of nodes, that are available initially.</param>
        /// <exception cref="ArgumentException"></exception>
        public RendezvousHash(Func<TKey,TNode, long> hashingFunction, IReadOnlyCollection<TNode> initialNodes)
        {
            if (initialNodes is null || initialNodes.Count == 0) throw new ArgumentException(nameof(initialNodes));
            
            this.hashingFunction = hashingFunction ?? throw new ArgumentException(nameof(initialNodes));
            nodes = new ConcurrentList<TNode>();
            nodes.AddRange(initialNodes);
        }


        /// <summary>
        /// Removes the node from the pool of available nodes.
        /// </summary>
        /// <param name="node">Node to remove from the poll of available nodes.</param>
        /// <returns>Indicator whether removal was successful.</returns>
        public bool Remove(TNode node)
        {
            return nodes.Remove(node);
        }

        /// <summary>
        /// Adds node to the poll of available nodes.
        /// </summary>
        /// <param name="node">Node to add to the pool of available nodes.</param>
        public void Add(TNode node)
        {
            nodes.Add(node);
        }


        /// <summary>
        /// Maps key to the node according to rendezvous hashing algorithm.
        /// </summary>
        /// <param name="key">Key to be mapped to the node.</param>
        /// <returns>A node to which key was mapped according to rendezvous hashing algorithm</returns>
        public TNode Get(TKey key)
        {
            var maxValue = long.MinValue;
            TNode result = default;

            foreach (var node in nodes)
            {
                var currentHash = hashingFunction(key, node);

                if (currentHash <= maxValue) continue;
                result = node;
                maxValue = currentHash;
            }

            return result;
        }
    }
}
