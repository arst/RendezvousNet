namespace RendezvousNet.Tests
{
    public class DummyKey : IProvideKeyValue
    {
        public DummyKey(string key)
        {
            KeyValue = key;
        }

        public string KeyValue { get; }
    }
}
