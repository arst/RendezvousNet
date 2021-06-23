using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RendezvousNet
{
    public class RendezvousHash<TKey, TNode> where TNode: IEquatable<TNode>
    {
        private readonly Func<TKey, TNode, long> hashingFunction;
        private readonly ConcurrentList<TNode> nodes;

        public RendezvousHash(Func<TKey,TNode, long> hashingFunction, IReadOnlyCollection<TNode> initialNodes)
        {
            if (initialNodes is null || initialNodes.Count == 0) throw new ArgumentException(nameof(initialNodes));
            
            this.hashingFunction = hashingFunction ?? throw new ArgumentException(nameof(initialNodes));
            nodes = new ConcurrentList<TNode>();
            nodes.AddRange(initialNodes);
        }

        public bool Remove(TNode node)
        {
            return nodes.Remove(node);
        }

        public void Add(TNode node)
        {
            nodes.Add(node);
        }

        public TNode Get(TKey key)
        {
            var maxValue = long.MinValue;
            TNode result = default;

            foreach (var node in nodes)
            {
                var currentHash = hashingFunction(key, node);

                if (currentHash > maxValue)
                {
                    result = node;
                    maxValue = currentHash;
                }
            }

            return result;
        }
    }
}
