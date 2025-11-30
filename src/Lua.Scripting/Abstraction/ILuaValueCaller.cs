using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaValueCaller
{
    ValueTask<LuaValue[]> CallValueAsync(LuaValue value, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default);
}
