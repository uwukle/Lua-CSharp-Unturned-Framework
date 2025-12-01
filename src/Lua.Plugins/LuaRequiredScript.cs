using System;

namespace Lua.Plugins;

public readonly record struct LuaRequiredScript(LuaRequiredScript.EFlags Flags, string Name, string Source)
{
    [Flags]
    public enum EFlags
    {
        None = 0,
        ExecuteOnLoad = 1,
        LoadFromSourceCode = 2,
        LoadFromFile = 4,
        ThrowIfNotFound = 8
    }
}
