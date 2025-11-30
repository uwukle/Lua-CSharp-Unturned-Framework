using System;
using System.Collections.Generic;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptProvider : ILuaScriptLoader, ILuaScriptUnloader, IDisposable
{
    IEnumerable<ILuaScript> Scripts { get; }

    bool Has(string name);

    ILuaScript? Get(string name);
}
