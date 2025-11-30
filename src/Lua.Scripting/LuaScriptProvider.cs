using Lua.Platforms;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting;

public sealed class LuaScriptProvider(LuaPlatform? platform = null) : ILuaScriptProvider, ILuaStringExecutor
{
    private const LuaScriptMetaData.ECreationFlags MEMORY_SCRIPT_CREATION_FLAGS = LuaScriptMetaData.ECreationFlags.Memory | LuaScriptMetaData.ECreationFlags.CSharp;
    private const LuaScriptMetaData.ECreationFlags FILE_SCRIPT_CREATION_FLAGS = LuaScriptMetaData.ECreationFlags.File | LuaScriptMetaData.ECreationFlags.CSharp;

    private readonly LuaState m_State = platform is null ? LuaState.Create() : LuaState.Create(platform);
    private readonly List<ILuaScript> m_ScriptsBridges = [];

    public IEnumerable<ILuaScript> Scripts => m_ScriptsBridges;

    public ILuaScript? Get(string name) => m_ScriptsBridges.Find(s => s.Name == name);

    public bool Has(string name) => m_ScriptsBridges.FindIndex(s => s.Name == name) is not -1;

    public ValueTask<ILuaScript> LoadAsync(string code, string name, CancellationToken cancellationToken = default)
    {
        var state = m_State;
        LuaTable env = [];
        LuaScript script = new(state, state.Load(code.AsSpan(), name, env), env, new(name, nameof(LuaScriptProvider), MEMORY_SCRIPT_CREATION_FLAGS));
        return new(InternalRegisterBridge(script, cancellationToken));
    }

    public async ValueTask<ILuaScript> LoadFromAsync(string path, CancellationToken cancellationToken = default)
    {
        var state = m_State;
        LuaTable env = [];
        LuaScript script = new(state, await state.LoadFileAsync(path, string.Empty, env, cancellationToken), env, new(Path.GetFileNameWithoutExtension(path), path, FILE_SCRIPT_CREATION_FLAGS));
        return await InternalRegisterBridge(script, cancellationToken);
    }

    public ValueTask UnloadAsync(string name, CancellationToken cancellationToken = default)
    {
        var bridges = m_ScriptsBridges;
        var index = bridges.FindIndex(s => s.Name == name);

        if (index is not -1) bridges.RemoveAt(index);

        return default;
    }

    public void Dispose()
    {
        m_State.Dispose();
    }

    private async Task<ILuaScript> InternalRegisterBridge(ILuaScript bridge, CancellationToken cancellationToken = default)
    {
        // TODO: Async event.
        m_ScriptsBridges.Add(bridge);
        return bridge;
    }

    public ValueTask<LuaValue[]> Execute(string code, CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_State.Load(code.AsSpan(), "dynamic", m_State.Environment), cancellationToken);
}
