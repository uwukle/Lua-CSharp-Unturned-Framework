using Lua.Runtime;
using Lua.Scripting.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Internal;

[LuaObject("Script")]
internal sealed partial class LuaScript(LuaState state, LuaClosure closure, LuaTable enviroment, LuaScriptMetaData metaData) : ILuaScript
{
    private readonly LuaState m_State = state;
    private readonly LuaClosure m_Closure = closure;
    private readonly LuaTable m_Environment = enviroment;

    [LuaMember]
    public string Name => MetaData.Name;

    [LuaMember]
    public LuaScriptMetaData MetaData => metaData;

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        SetupLocalVariables();
        return m_State.ExecuteAsync(m_Closure, cancellationToken);
    }

    [LuaIgnoreMember]
    public bool TryGetValue(LuaValue name, out LuaValue value) => m_Environment.TryGetValue(name, out value) && value.Type is not LuaValueType.Nil;

    [LuaMember]
    public LuaValue GetValue(LuaValue name) => m_Environment[name];

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> CallAsync(LuaValue name, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => CallValueAsync(GetValue(name), arguments, cancellationToken);

    [LuaMember]
    public void SetValue(LuaValue name, LuaValue value) => m_Environment[name] = value;

    [LuaMember]
    public void RemoveValue(LuaValue name) => m_Environment[name] = LuaValue.Nil;

    [LuaMember]
    public void Clear() => m_Environment.Clear();

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> ExecuteAsync(string code, CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_State.Load(code.AsSpan(), "dynamic", m_Environment), cancellationToken);

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> CallValueAsync(LuaValue value, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => m_State.CallAsync(value, arguments, cancellationToken);

    private void SetupLocalVariables() => SetValue("LocalMetaData", MetaData);
}
