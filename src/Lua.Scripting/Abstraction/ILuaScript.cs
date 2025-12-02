using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScript : ILuaStringExecutor, ILuaValueCaller, ILuaUserData
{
    string Name { get; }

    LuaScriptMetaData MetaData { get; }

    LuaTable Environment { get; }

    ValueTask<LuaValue[]> ExecuteAsync(CancellationToken cancellationToken = default);
}
