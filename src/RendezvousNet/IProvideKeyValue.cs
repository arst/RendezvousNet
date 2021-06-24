namespace RendezvousNet
{
    /// <summary>
    /// Implement this node to provide a key value to be used in (node,key) pair hash calculation.
    /// </summary>
    public interface IProvideKeyValue
    {
        /// <summary>
        /// Key value to be used in (node,key) pair hash calculation.
        /// </summary>
        public string KeyValue { get; }
    }
}