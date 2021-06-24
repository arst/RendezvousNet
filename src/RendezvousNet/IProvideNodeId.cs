namespace RendezvousNet
{
    /// <summary>
    /// Implement this node to provide an id for the node to be used in (node,key) pair hash calculation.
    /// </summary>
    public interface IProvideNodeId
    {
        /// <summary>
        /// Node id for the node to be used in (node,key) pair hash calculation.
        /// </summary>
        public string NodeId { get; }
    }
}
