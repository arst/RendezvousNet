using BenchmarkDotNet.Running;

namespace RendezvousNet.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RendezvousHashBenchmark>();
        }
    }
}