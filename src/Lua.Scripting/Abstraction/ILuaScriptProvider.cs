using System;
using System.Collections.Generic;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptProvider : ILuaScriptLoader, ILuaScriptUnloader, IAsyncDisposable
{
    IEnumerable<ILuaScript> Scripts { get; }

    bool Has(string name);

    ILuaScript? Get(string name);
}
