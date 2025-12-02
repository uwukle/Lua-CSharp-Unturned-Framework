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

    [LuaMember]
    public string Name => MetaData.Name;

    [LuaMember]
    public LuaScriptMetaData MetaData => metaData;

    [LuaMember]
    public LuaTable Environment => enviroment;

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> CallValueAsync(LuaValue value, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => m_State.CallAsync(value, arguments, cancellationToken);

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> ExecuteAsync(CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_Closure, cancellationToken);

    [LuaIgnoreMember]
    public ValueTask<LuaValue[]> ExecuteAsync(string code, CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_State.Load(code, "dynamic", Environment), cancellationToken);
}
