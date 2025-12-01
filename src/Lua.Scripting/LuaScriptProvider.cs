using Lua.Platforms;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting;

public sealed class LuaScriptProvider(LuaPlatform? platform = null) : ILuaScriptProvider, ILuaStringExecutor, ILuaValueCaller
{
    private const LuaScriptMetaData.ECreationFlags MEMORY_SCRIPT_CREATION_FLAGS = LuaScriptMetaData.ECreationFlags.Memory | LuaScriptMetaData.ECreationFlags.CSharp;
    private const LuaScriptMetaData.ECreationFlags FILE_SCRIPT_CREATION_FLAGS = LuaScriptMetaData.ECreationFlags.File | LuaScriptMetaData.ECreationFlags.CSharp;

    private readonly LuaState m_State = platform is null ? LuaState.Create() : LuaState.Create(platform);
    private readonly List<ILuaScript> m_Scripts = [];

    public IEnumerable<ILuaScript> Scripts => m_Scripts;

    public bool HasByName(string name) => m_Scripts.FindIndex(s => s.Name == name) is not -1;

    public ILuaScript? GetByName(string name) => m_Scripts.Find(s => s.Name == name);

    public ValueTask<ILuaScript> LoadAsync(string code, string name, CancellationToken cancellationToken = default)
    {
        var state = m_State;
        LuaTable env = [];
        LuaScript script = new(state, state.Load(code.AsSpan(), name, env), env, new(name, code, MEMORY_SCRIPT_CREATION_FLAGS));
        return new(InternalRegisterScriptAsync(script, cancellationToken));
    }

    public async ValueTask<ILuaScript> LoadFromAsync(string path, string name, CancellationToken cancellationToken = default)
    {
        var state = m_State;
        LuaTable env = [];
        LuaScript script = new(state, await state.LoadFileAsync(path, string.Empty, env, cancellationToken), env, new(name, path, FILE_SCRIPT_CREATION_FLAGS));
        return await InternalRegisterScriptAsync(script, cancellationToken);
    }

    public ValueTask UnloadByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var scripts = m_Scripts;
        var index = scripts.FindIndex(s => s.Name == name);

        if (index is not -1) scripts.RemoveAt(index);

        return default;
    }

    public ValueTask UnloadBySourceAsync(string source, CancellationToken cancellationToken = default)
    {
        var scripts = m_Scripts;
        var index = scripts.FindIndex(s => s.MetaData.Source == source);

        if (index is not -1) scripts.RemoveAt(index);

        return default;
    }

    public ValueTask<LuaValue[]> ExecuteAsync(string code, CancellationToken cancellationToken = default) => m_State.ExecuteAsync(m_State.Load(code.AsSpan(), "dynamic", m_State.Environment), cancellationToken);

    public ValueTask<LuaValue[]> CallValueAsync(LuaValue value, ReadOnlySpan<LuaValue> arguments, CancellationToken cancellationToken = default) => m_State.CallAsync(value, arguments, cancellationToken);

    public ValueTask DisposeAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Async event.
        m_State.Dispose();
        return default;
    }

    public ValueTask DisposeAsync() => DisposeAsync(default);

    private async Task<ILuaScript> InternalRegisterScriptAsync(ILuaScript bridge, CancellationToken cancellationToken = default)
    {
        // TODO: Async event.
        m_Scripts.Add(bridge);
        return bridge;
    }

    public ILuaScript? GetBySource(string source) => m_Scripts.Find(s => s.MetaData.Source == source);
}
