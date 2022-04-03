using BenchmarkDotNet.Attributes;

namespace Ampl.Core.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class FastReverseBenchmark
    {
        private const string _testString = "This is a test string";

        [Benchmark]
        public string FastReverseV1()
        {
            return _testString.FastReverse();
        }

        //[Benchmark]
        //public string FastReverseV2()
        //{
        //    return _testString.FastReverseOptimized();
        //}
    }
}
