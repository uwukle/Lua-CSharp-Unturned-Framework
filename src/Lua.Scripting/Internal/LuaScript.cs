using Lua.Runtime;
using Lua.Scripting.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Internal;

internal sealed class LuaScript(LuaState state, LuaClosure closure, LuaTable enviroment, LuaScriptMetaData metaData) : ILuaScript
{
    private readonly LuaState m_State = state;
    private readonly LuaClosure m_Closure = closure;
    private readonly LuaTable m_Environment = enviroment;

    public string Name => MetaData.Name;

    public LuaScriptMetaData MetaData => metaData;

    public ValueTask<LuaValue[]> CallAsync(LuaValue name, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => m_State.CallAsync(GetValue(name), arguments, cancellationToken);

    public void Clear() => m_Environment.Clear();

    public ValueTask<LuaValue[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        SetupLocalVariables();
        return m_State.ExecuteAsync(m_Closure, cancellationToken);
    }

    public LuaValue GetValue(LuaValue name) => m_Environment[name];

    public void SetValue(LuaValue name, LuaValue value) => m_Environment[name] = value;

    public bool TryGetValue(LuaValue name, out LuaValue value) => m_Environment.TryGetValue(name, out value) && value.Type is not LuaValueType.Nil;

    public void RemoveValue(LuaValue name) => m_Environment[name] = LuaValue.Nil;

    public ValueTask<LuaValue[]> ReloadAsync(CancellationToken cancellationToken = default)
    {
        Clear();
        return ExecuteAsync(cancellationToken);
    }

    public ValueTask<LuaValue[]> Execute(string code, CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_State.Load(code.AsSpan(), "dynamic", m_Environment), cancellationToken);

    private void SetupLocalVariables() => SetValue("LocalMetaData", MetaData);
}
