using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptProvider : ILuaScriptLoader, ILuaScriptUnloader, IAsyncDisposable
{
    IEnumerable<ILuaScript> Scripts { get; }

    bool HasByName(string name);

    ILuaScript? GetByName(string name);

    ILuaScript? GetBySource(string source);

    ValueTask DisposeAsync(CancellationToken cancellationToken = default);
}
