using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScript : ILuaStringExecutor, ILuaValueCaller, ILuaUserData
{
    string Name { get; }

    LuaScriptMetaData MetaData { get; }

    ValueTask<LuaValue[]> ExecuteAsync(CancellationToken cancellationToken = default);

    bool TryGetValue(LuaValue name, out LuaValue value);

    LuaValue GetValue(LuaValue name);

    ValueTask<LuaValue[]> CallAsync(LuaValue name, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default);

    void SetValue(LuaValue name, LuaValue value);

    void RemoveValue(LuaValue name);

    void Clear();
}
