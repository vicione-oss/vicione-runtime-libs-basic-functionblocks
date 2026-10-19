using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Logging.Abstractions;
using ViciOne.Analysis;
using ViciOne.Analysis.Benchmarks;
using ViciOne.Core.Contracts;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Dataflow.DataModel;
using ViciOne.Core.Dataflow.DataModel.Editors;
using ViciOne.Core.Dataflow.DataModel.Extensions;
using ViciOne.Core.Dataflow.DataModel.Generation;
using ViciOne.Core.Dataflow.Pooling;
using ViciOne.Core.Runtime.Execution;
using ViciOne.Engine;

// IntervalStatistic blocks in real RuntimeEngines. Every block's six outputs are linked to the inputs of three Add blocks.
// Usage: ViciOne.Analysis.Benchmarks [engines=4] [blocksPerEngine=1250] [compressionMs=10000] [changesPerSecond=1] [cycleMs=10] [seconds=60] [warmupSeconds=15]
BenchmarkOptions options = BenchmarkOptions.Parse(args);
Console.WriteLine(options);

Assembly[] designAssemblies = [ typeof(IntervalStatistic).Assembly, typeof(ViciOne.Math.Add).Assembly];
RelevantAssemblies relevant = AssemblyLoader.CreateRelevantAssemblies(typeof(IntervalStatistic).Assembly);
Assembly[] assemblies = designAssemblies.Concat(relevant.AdditionalAssemblies).Distinct().ToArray();
List<FunctionBlockDesign> designs = FunctionBlockDesignGenerator.GenerateFunctionBlockDesigns(
    RuntimeFunctionBlockAssemblyAnalyzer.GetFunctionBlockDesigns(designAssemblies), new PoolingFactory(relevant.AdditionalAssemblies)).ToList();
FunctionBlockDesign statisticDesign = designs.Single(d => d.Name == nameof(IntervalStatistic));
FunctionBlockDesign addDesign = designs.Single(d => d.Name == nameof(ViciOne.Math.Add));

Stopwatch setup = Stopwatch.StartNew();
List<EngineUnderTest> engines = Enumerable.Range(0, options.Engines).Select(_ => EngineUnderTest.Create(statisticDesign, addDesign, options, assemblies)).ToList();
Console.WriteLine($"setup: {setup.Elapsed.TotalSeconds:F1} s for {engines.Count * options.BlocksPerEngine * 4} function blocks");

Process process = Process.GetCurrentProcess();
List<Thread> threads = engines.Select(e => new Thread(() => e.Run(options)) { Name = "engine", IsBackground = true }).ToList();
threads.ForEach(t => t.Start());

Thread.Sleep(TimeSpan.FromSeconds(options.WarmupSeconds));
GC.Collect();
process.Refresh();
TimeSpan cpu0 = process.TotalProcessorTime;
long alloc0 = GC.GetTotalAllocatedBytes();
(int, int, int) gc0 = (GC.CollectionCount(0), GC.CollectionCount(1), GC.CollectionCount(2));
TimeSpan pause0 = GC.GetTotalPauseDuration();
engines.ForEach(e => e.StartMeasuring());
Stopwatch wall = Stopwatch.StartNew();

Thread.Sleep(TimeSpan.FromSeconds(options.Seconds));
engines.ForEach(e => e.Stop());
threads.ForEach(t => t.Join());
process.Refresh();

double[] cycles = engines.SelectMany(e => e.CycleMilliseconds).Order().ToArray();
double Percentile(double p) => cycles.Length == 0 ? 0 : cycles[Math.Min(cycles.Length - 1, (int)(p * cycles.Length))];
double cpu = (process.TotalProcessorTime - cpu0).TotalMilliseconds;
Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
    $"""
    CPU:            {cpu / wall.Elapsed.TotalMilliseconds * 100:F1} % of one core
    allocated:      {(GC.GetTotalAllocatedBytes() - alloc0) / 1048576.0:F1} MB
    GC gen0/1/2:    {GC.CollectionCount(0) - gc0.Item1}/{GC.CollectionCount(1) - gc0.Item2}/{GC.CollectionCount(2) - gc0.Item3}, pauses {(GC.GetTotalPauseDuration() - pause0).TotalMilliseconds:F0} ms
    cycles:         {cycles.Length}, over {options.CycleMs} ms: {cycles.Count(c => c > options.CycleMs)}
    cycle ms:       p50 {Percentile(0.5):F2}, p99 {Percentile(0.99):F2}, p99.9 {Percentile(0.999):F2}, max {(cycles.Length == 0 ? 0 : cycles[^1]):F2}
    working set:    {process.PeakWorkingSet64 / 1048576.0:F0} MB peak
    completions:    {engines.Sum(e => e.Completions)} (expected ~{options.Engines * options.BlocksPerEngine * options.Seconds * 1000 / options.CompressionMs})
    """));
Console.WriteLine("longest cycles per engine: " + string.Join(" | ", engines.Select(e => string.Join(", ", e.CycleMilliseconds.OrderDescending().Take(3).Select(c => c.ToString("F1", CultureInfo.InvariantCulture))))));
engines.ForEach(e => e.Dispose());

namespace ViciOne.Analysis.Benchmarks
{
    internal sealed record BenchmarkOptions(int Engines, int BlocksPerEngine, int CompressionMs, int ChangesPerSecond, int CycleMs, int Seconds, int WarmupSeconds)
    {
        public static BenchmarkOptions Parse(string[] args)
        {
            int Arg(int index, int fallback) => args.Length > index ? int.Parse(args[index], CultureInfo.InvariantCulture) : fallback;
            return new(Arg(0, 4), Arg(1, 1250), Arg(2, 10000), Arg(3, 1), Arg(4, 10), Arg(5, 60), Arg(6, 15));
        }
    }

    internal sealed class EngineUnderTest : IDisposable
    {
        private readonly InputCommunication _communication;
        private readonly RuntimeEngine _engine;
        private readonly Guid[] _incomingLinkIds;
        private IntervalStatistic[] _statistics = [];
        private double[] _lastSums = [];
        private volatile bool _measuring;
        private volatile bool _stopped;

        private EngineUnderTest(RuntimeEngine engine, InputCommunication communication, Guid[] incomingLinkIds)
        {
            _engine = engine;
            _communication = communication;
            _incomingLinkIds = incomingLinkIds;
        }

        public List<double> CycleMilliseconds { get; } = [];

        public long Completions { get; private set; }

        [SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling", Justification = "Builds a complete dataflow and engine in one place.")]
        public static EngineUnderTest Create(FunctionBlockDesign statisticDesign, FunctionBlockDesign addDesign, BenchmarkOptions options, IEnumerable<Assembly> assemblies)
        {
            using DataflowEditor editor = new();
            editor.AddFunctionBlockDesign(statisticDesign);
            editor.AddFunctionBlockDesign(addDesign);
            Guid[] incomingLinkIds = new Guid[options.BlocksPerEngine];

            for (int i = 0; i < options.BlocksPerEngine; i++)
            {
                FunctionBlockEditor statistic = new(statisticDesign);
                statistic.SetName($"Statistic{i}");
                editor.RootContainerEditor.AddFunctionBlock(statistic);
                ISetting setting = statistic.FunctionBlock.Settings.Single(s => s.Design.Name == nameof(IntervalStatistic.Settings.CompressionTime));
                setting.Value = options.CompressionMs;

                ConnectorInput<double> input = (ConnectorInput<double>)statistic.FunctionBlock.GetInputs().Single(c => c.Design.Name == nameof(IntervalStatistic.ProcessDataInputs.Value));
                ExternalIncomingLink<double, double, double> incoming = new ExternalIncomingLinkEditor<double, double, double>(input).Link;
                editor.AddExternalIncomingLink(incoming);
                incomingLinkIds[i] = incoming.Id;

                List<ConnectorOutput<double>> outputs = statistic.FunctionBlock.GetOutputs().OfType<ConnectorOutput<double>>().OrderBy(c => c.Design.Name, StringComparer.Ordinal).ToList();
                for (int a = 0; a < 3; a++)
                {
                    FunctionBlockEditor add = new(addDesign);
                    add.SetName($"Add{i}_{a}");
                    add.SetRunMode(RunMode.Change);
                    editor.RootContainerEditor.AddFunctionBlock(add);
                    List<ConnectorInput<double>> addInputs = add.FunctionBlock.GetInputs().OfType<ConnectorInput<double>>().OrderBy(c => c.Design.Name, StringComparer.Ordinal).ToList();
                    for (int k = 0; k < 2; k++)
                    {
                        ConnectorOutput<double> source = outputs[(a * 2) + k];
                        editor.AddInternalLink(new InternalLinkEditor<double, double>(source, addInputs[k]).Link);
                    }
                }
            }

            InputCommunication communication = new();
            DefaultUniqueNameProvider names = new(NameGenerator.CreateUniqueNames(editor.Dataflow));
            RuntimeEngine engine = new(editor.Dataflow, NullLoggerFactory.Instance, assemblies, assemblies, names, communicationManager: communication);
            engine.Start();
            EngineUnderTest result = new(engine, communication, incomingLinkIds);
            result._statistics = [.. GetRuntimeFunctionBlocks(engine).OfType<IntervalStatistic>()];
            result._lastSums = new double[result._statistics.Length];
            return result;
        }

        public void Run(BenchmarkOptions options)
        {
            int cyclesPerChange = System.Math.Max(1, 1000 / options.CycleMs / options.ChangesPerSecond);
            Stopwatch clock = Stopwatch.StartNew();
            List<ChangedEntry> entries = [];
            for (long cycle = 0; _stopped == false; cycle++)
            {
                entries.Clear();
                for (int i = (int)(cycle % cyclesPerChange); i < _incomingLinkIds.Length; i += cyclesPerChange)
                    entries.Add(new ChangedEntry { Id = _incomingLinkIds[i], Time = DateTime.UtcNow, Validity = Validity.Valid, Value = ((cycle * 7919) + (i * 104729)) % 10000 / 100.0 });
                if (entries.Count > 0)
                    _communication.Raise(entries.ToArray());

                long start = Stopwatch.GetTimestamp();
                _engine.ProcessCycle();
                double elapsed = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
                if (_measuring)
                {
                    CycleMilliseconds.Add(elapsed);
                    Completions += CountCompletions();
                }

                long next = (cycle + 1) * options.CycleMs;
                long wait = next - clock.ElapsedMilliseconds;
                if (wait > 0)
                    Thread.Sleep((int)wait);
            }
        }

        private int CountCompletions()
        {
            int count = 0;
            for (int i = 0; i < _statistics.Length; i++)
            {
                double sum = _statistics[i].ProcessDataOutputs.Sum.Value;
                if (sum != _lastSums[i])
                {
                    _lastSums[i] = sum;
                    count++;
                }
            }

            return count;
        }

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_runtimeFunctionBlocks")]
        private static extern ref IRuntimeFunctionBlock[] GetRuntimeFunctionBlocks(RuntimeEngine engine);

        public void StartMeasuring()
            => _measuring = true;

        public void Stop()
            => _stopped = true;

        public void Dispose()
        {
            _engine.Stop();
            _engine.Dispose();
        }
    }

    internal sealed class InputCommunication : ICommunicationManager
    {
        public event EventHandler<IEnumerable<ILinkChangedEntry>>? ChangedEntriesReceived;

        public void HandleChangedOutgoingLinks(IEnumerable<ILinkChangedEntry> externalLinkEntries)
        {
        }

        public void Raise(IEnumerable<ILinkChangedEntry> entries)
            => ChangedEntriesReceived?.Invoke(this, entries);
    }
}
