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

public abstract class RuntimeFunctionBlockTests<TBlock>(DataflowInfo dataflowInfo, IEnumerable<Assembly> assemblies) : IDisposable where TBlock : RuntimeFunctionBlock<TBlock>, new()
{
    private bool _disposed;

    protected virtual bool AllowErrorsInProcessCycle { get; set; }
    protected IEnumerable<Assembly> Assemblies { get; set; } = assemblies;
    protected FunctionBlockExecutor? BlockExecutor { get; private set; }
    protected ITestLoggerFactory LoggerFactory { get; } = TestLoggerFactory.Create();
    protected DataflowInfo DataflowInfo { get; set; } = dataflowInfo;

    protected RuntimeFunctionBlockTests() : this(GenerateDefaultDataflow()) { }

    protected RuntimeFunctionBlockTests((DataflowInfo DataflowInfo, IEnumerable<Assembly> Assemblies) pair) : this(pair.DataflowInfo, pair.Assemblies) { }

    protected static RelevantAssemblies CreateDefaultAssemblies()
        => AssemblyLoader.CreateRelevantAssemblies(typeof(TBlock).Assembly.Location);

    protected virtual void ChangeEnabled(bool enabled)
        => ChangeInputValue(SystemConnectorNames.FunctionBlockEnabledName, enabled);

    protected virtual void ChangeInputValue(string connectorName, object newValue)
    {
        var input = GetInput(connectorName);
        if (BlockExecutor is not null)
            BlockExecutor.ChangeInputValue(input, newValue);
        else
            input.Value = newValue;
    }

    protected virtual void ChangePersistentVariableValue(string variableName, object newValue)
    {
        var variable = GetPersistentVariable(variableName);
        if (BlockExecutor is not null)
            BlockExecutor.ChangePersistentVariableValue(variable, newValue);
        else
            variable.Value = newValue;
    }

    protected virtual void ChangeSettingValue(string settingName, object newValue)
    {
        var setting = GetSetting(settingName);
        if (BlockExecutor is not null)
            BlockExecutor.ChangeSettingValue(setting, newValue);
        else
        {
            setting.Value = newValue;
            setting.Input.Value = newValue;
        }
    }

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

        BlockExecutor?.Dispose();
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
        => DataflowInfo.Inputs.First(x => string.Equals(x.Connector.Design.Name, connectorName, StringComparison.Ordinal)).Connector;

    protected virtual DateTime GetInputChangedTime(string connectorName)
    {
        ThrowIfEngineNotStarted();
        return BlockExecutor!.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Time;
    }

    protected virtual Validity GetInputValidity(string connectorName)
    {
        ThrowIfEngineNotStarted();
        return BlockExecutor!.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Validity;
    }

    protected virtual TValue GetInputValue<TValue>(string connectorName)
        => BlockExecutor is not null
            ? (TValue)BlockExecutor.GetInputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Value!
            : (TValue)GetInput(connectorName).Value!;

    protected virtual IConnector GetOutput(string connectorName)
        => DataflowInfo.Outputs.First(x => string.Equals(x.Connector.Design.Name, connectorName, StringComparison.Ordinal)).Connector;

    protected virtual DateTime GetOutputChangedTime(string connectorName)
    {
        ThrowIfEngineNotStarted();
        return BlockExecutor!.GetOutputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Time;
    }

    protected virtual Validity GetOutputValidity(string connectorName)
    {
        ThrowIfEngineNotStarted();
        return BlockExecutor!.GetOutputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Validity;
    }

    protected virtual TValue GetOutputValue<TValue>(string connectorName)
        => BlockExecutor is not null
            ? (TValue)BlockExecutor.GetOutputValues().First(x => string.Equals(x.Key.Design.Name, connectorName, StringComparison.Ordinal)).Value.Value!
            : (TValue)GetOutput(connectorName).Value!;

    protected virtual IPersistentVariable GetPersistentVariable(string variableName)
        => DataflowInfo.PersistentVariables.First(x => string.Equals(x.Design.Name, variableName, StringComparison.Ordinal));

    protected virtual TValue GetPersistentVariableValue<TValue>(string variableName)
        => BlockExecutor is not null
            ? (TValue)BlockExecutor.GetPersistentVariableValues().First(x => string.Equals(x.Key.Design.Name, variableName, StringComparison.Ordinal)).Value.Value!
            : (TValue)GetPersistentVariable(variableName).Value!;

    protected TBlock GetRuntimeFunctionBlock()
    {
        ThrowIfEngineNotStarted();

        var engine = RuntimeFunctionBlockExtractor.GetEngine(BlockExecutor!);
        var functionBlocks = RuntimeFunctionBlockExtractor.GetRuntimeFunctionBlocks(engine);

        return functionBlocks.OfType<TBlock>().Single();
    }

    protected virtual ISetting GetSetting(string settingName)
        => DataflowInfo.Settings.First(x => string.Equals(x.Design.Name, settingName, StringComparison.Ordinal));

    protected virtual TValue GetSettingValue<TValue>(string settingName)
        => BlockExecutor is not null
            ? (TValue)BlockExecutor.GetSettingValues().First(x => string.Equals(x.Key.Design.Name, settingName, StringComparison.Ordinal)).Value.Value!
            : (TValue)GetSetting(settingName).Value!;

    protected virtual void ProcessCycle()
    {
        if (BlockExecutor is null)
            StartEngine();

        BlockExecutor!.ProcessCycle();
        if (AllowErrorsInProcessCycle is false)
            ThrowIfErrorsLogged();
    }

    protected virtual void StartEngine()
    {
        BlockExecutor = new FunctionBlockExecutor(DataflowInfo, Assemblies, LoggerFactory);
        BlockExecutor.StartEngine();
        ThrowIfEngineNotStarted();
    }

    protected virtual void ThrowIfEngineNotStarted()
    {
        if (BlockExecutor is null)
            throw new InvalidStateException("Engine is not started.");

        if (BlockExecutor.EngineState is not EngineState.Starting and not EngineState.Running)
            throw new InvalidStateException($"Invalid engine state found. Expected '{EngineState.Starting}' or '{EngineState.Running}' but found '{BlockExecutor.EngineState}'");
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
