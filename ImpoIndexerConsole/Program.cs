using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ImpoIndexerConsole;
using ImpoIndexerConsole.Indexers;
using ImpoIndexerConsole.Model;

var app = new ClaudeApplication();
await app.Run();
//BenchmarkRunner.Run<Benchmarks>();

//Dictionary<string, IEnumerable<int>> dic = [];
//int template;


//dic.Add("arquivo1.pdf", Enumerable.Range(1, 16));
//dic.Add("arquivo2.pdf", Enumerable.Range(1, 16));
//dic.Add("arquivo3.pdf", Enumerable.Range(1, 16));
//template = 4;

//var calc = new CutStack();
//calc.CalcularStruct(dic, template);