using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using MELT;
using Microsoft.Extensions.Logging;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Execution;
using ViciOne.Engine;

public abstract class RuntimeFunctionBlockTests<TBlock> : IDisposable where TBlock : RuntimeFunctionBlock<TBlock>, new()
{
    private bool _disposed;

    protected virtual bool AllowErrorsInProcessCycle { get; set; }
    protected FunctionBlockExecutor BlockExecutor { get; }
    protected ITestLoggerFactory LoggerFactory { get; }

    protected RuntimeFunctionBlockTests() : this(GenerateDefaultDataflow()) { }

    protected RuntimeFunctionBlockTests((DataflowInfo DataflowInfo, IEnumerable<Assembly> Assemblies) pair) : this(pair.DataflowInfo, pair.Assemblies) { }

    protected RuntimeFunctionBlockTests(DataflowInfo dataflowInfo, IEnumerable<Assembly> assemblies)
    {
        LoggerFactory = TestLoggerFactory.Create();
        BlockExecutor = new FunctionBlockExecutor(dataflowInfo, assemblies, LoggerFactory);
        BlockExecutor.StartEngine();

        if (BlockExecutor.EngineState != EngineState.Starting)
            throw new InvalidStateException($"Invalid engine state found. Expected '{EngineState.Starting}' but found '{BlockExecutor.EngineState}'");

        BlockExecutor.ProcessCycle();
    }

    protected static RelevantAssemblies CreateDefaultAssemblies()
        => AssemblyLoader.CreateRelevantAssemblies(typeof(TBlock).Assembly.Location);

    protected virtual void ChangeEnabled(bool enabled)
        => BlockExecutor.ChangeInputValue(GetInput(SystemConnectorNames.FunctionBlockEnabledName), enabled);

    protected virtual void ChangeInputValue(string connectorName, object newValue)
        => BlockExecutor.ChangeInputValue(GetInput(connectorName), newValue);

    protected virtual void ChangePersistentVariableValue(string variableName, object newValue)
        => BlockExecutor.ChangePersistentVariableValue(GetPersistentVariable(variableName), newValue);

    protected virtual void ChangeSettingValue(string settingName, object newValue)
        => BlockExecutor.ChangeSettingValue(GetSetting(settingName), newValue);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;
        if (disposing is false)
            return;

        BlockExecutor.Dispose();
        LoggerFactory.Dispose();
    }

    protected static (DataflowInfo, IEnumerable<Assembly>) GenerateDefaultDataflow()
    {
        var assemblies = CreateDefaultAssemblies();
        return (GenerateDefaultDataflow(assemblies), assemblies.AllAssemblies);
    }

    protected static DataflowInfo GenerateDefaultDataflow(RelevantAssemblies assemblies)
    {
        var @namespace = typeof(TBlock).Namespace;
        var name = typeof(TBlock).Name;
        return DataflowGenerator.GenerateDataflow(assemblies, designs => designs.First(design =>
            string.Equals(design.Namespace, @namespace, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(design.Name, name, StringComparison.OrdinalIgnoreCase)));
    }

    protected virtual IConnector GetInput(string connectorName)
        => BlockExecutor.GetInputs().First(x => string.Equals(x.Design.Name, connectorName, StringComparison.Ordinal));

    protected virtual DateTime GetInputChangedTime(string connectorName)
        => BlockExecutor.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Time;

    protected virtual Validity GetInputValidity(string connectorName)
        => BlockExecutor.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Validity;

    protected virtual TValue GetInputValue<TValue>(string connectorName)
        => (TValue)BlockExecutor.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Value!;

    protected virtual Validity GetOutputValidity(string connectorName)
        => BlockExecutor.GetOutputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Validity;

    protected virtual TValue GetOutputValue<TValue>(string connectorName)
        => (TValue)BlockExecutor.GetOutputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Value!;

    protected virtual IPersistentVariable GetPersistentVariable(string variableName)
        => BlockExecutor.GetPersistentVariables().First(x => string.Equals(x.Design.Name, variableName, StringComparison.Ordinal));

    protected virtual TValue GetPersistentVariableValue<TValue>(string variableName)
        => (TValue)BlockExecutor.GetPersistentVariableValues().First(x => string.Equals(x.Key.Design.Name, variableName, StringComparison.Ordinal)).Value.Value!;

    protected TBlock GetRuntimeFunctionBlock()
    {
        var engine = RuntimeFunctionBlockExtractor.GetEngine(BlockExecutor);
        var functionBlocks = RuntimeFunctionBlockExtractor.GetRuntimeFunctionBlocks(engine);

        return functionBlocks.OfType<TBlock>().Single();
    }

    protected virtual ISetting GetSetting(string settingName)
        => BlockExecutor.GetSettings().First(x => string.Equals(x.Design.Name, settingName, StringComparison.Ordinal));

    protected virtual TValue GetSettingValue<TValue>(string settingName)
        => (TValue)BlockExecutor.GetSettingValues().First(x => string.Equals(x.Key.Design.Name, settingName, StringComparison.Ordinal)).Value.Value!;

    protected virtual void ProcessCycle()
    {
        BlockExecutor.ProcessCycle();
        if (AllowErrorsInProcessCycle is false)
            ThrowIfErrorsLogged();
    }

    protected virtual void ThrowIfErrorsLogged()
    {
        var errors = LoggerFactory.Sink.LogEntries.Where(x => x.LogLevel is LogLevel.Error or LogLevel.Critical).ToArray();
        if (errors.Length == 0)
            return;
        if (errors.Length > 1)
            throw new AggregateException("Multiple errors occured while processing cycle.", errors.Select(GetException));
        else
            ExceptionDispatchInfo.Capture(GetException(errors.Single())).Throw();

        static Exception GetException(LogEntry logEntry)
            => logEntry.Exception ?? new MessageOnlyException(logEntry.Message!);
    }
}

[SuppressMessage("Design", "CA1032:Standardausnahmekonstruktoren implementieren", Justification = "Die Ausnahme wird nur von RuntimeFunctionBlockTests erzeugt und ist nur mit einer Nachricht aussagekräftig.")]
public class InvalidStateException(string message) : Exception(message) { }

[SuppressMessage("Design", "CA1032:Standardausnahmekonstruktoren implementieren", Justification = "Die Ausnahme wird nur von RuntimeFunctionBlockTests erzeugt und ist nur mit einer Nachricht aussagekräftig.")]
public class MessageOnlyException(string message) : Exception(message) { }

public class RuntimeFunctionBlockExtractor
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_runtimeEngine")]
    public static extern ref RuntimeEngine GetEngine(FunctionBlockExecutor functionBlockExecutor);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_runtimeFunctionBlocks")]
    public static extern ref IRuntimeFunctionBlock[] GetRuntimeFunctionBlocks(RuntimeEngine runtimeEngine);
}
