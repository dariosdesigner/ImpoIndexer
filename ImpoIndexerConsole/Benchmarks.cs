using BenchmarkDotNet.Attributes;
using ImpoIndexerConsole.Indexers;

namespace ImpoIndexerConsole;

[MemoryDiagnoser(true)]
public class Benchmarks
{
    readonly Dictionary<string, IEnumerable<int>> dic = [];
    int template;
    [GlobalSetup]
    public void Setup()
    {
        var index = 1;
        
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16)); 
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));
        dic.Add($"arquivo{index++}.pdf", Enumerable.Range(1, 16));

        template = 4;
    }

   
    //[Benchmark]
    //public void RearrangeListStruct()
    //{
    //    var indexer = new CutStack();
    //    indexer.CalcularStruct(dic, template);
    //}
    [Benchmark]
    public void RearrangeArrayPoolStruct()
    {
        var indexer = new CutStack();
        indexer.CalcularArrayPoolStruct(dic, template);
    }  
}
