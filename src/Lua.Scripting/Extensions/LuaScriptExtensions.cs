using Lua.Scripting.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Extensions;

public static class LuaScriptExtensions
{
    public static ValueTask<LuaValue[]> CallIfCanAsync(this ILuaScript script, LuaValue name, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => script.TryGetValue(name, out var callee) ? script.CallValueAsync(callee, arguments, cancellationToken) : new([]);
}
