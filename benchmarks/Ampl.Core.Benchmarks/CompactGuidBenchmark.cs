using BenchmarkDotNet.Attributes;

namespace Ampl.Core.Benchmarks;

[MemoryDiagnoser(false)]
public class CompactGuidBenchmark
{
    private static readonly Guid _guid = new("48e42ef1-ddcf-4e1f-9f93-d31ab170dd03");
    private static readonly string _compactGuid = "8S7kSM_dH06fk9MasXDdAw";

    //[Benchmark]
    //public string ToCompactGuidV1()
    //{
    //    return _guid.ToCompactStringUnoptimized();
    //}

    [Benchmark]
    public string ToCompactGuidV2()
    {
        return _guid.ToCompactString();
    }

    //[Benchmark]
    //public Guid FromCompactGuidV1()
    //{
    //    return CompactGuid.ParseUnoptimized(_compactGuid);
    //}

    [Benchmark]
    public Guid FromCompactGuidV2()
    {
        return CompactGuid.Parse(_compactGuid);
    }

}
