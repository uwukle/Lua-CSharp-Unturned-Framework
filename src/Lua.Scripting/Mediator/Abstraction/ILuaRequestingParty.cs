using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator.Abstraction;

public interface ILuaRequestingParty
{
    ValueTask<LuaValue[]> RequestAsync(string callback, ReadOnlySpan<LuaValue> requestArguments, CancellationToken cancellationToken = default);
}
