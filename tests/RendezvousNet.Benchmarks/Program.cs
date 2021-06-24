using BenchmarkDotNet.Running;

namespace RendezvousNet.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RendezvousHashBenchmark>();
        }
    }
}
