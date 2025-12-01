using Lua.Scripting.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Extensions;

public static class LuaScriptProviderExtensions
{
    public static ValueTask<ILuaScript> ReloadAsync(ILuaScriptProvider provider, string name, CancellationToken cancellationToken = default)
    {
        var script = provider.GetByName(name) ?? throw new ArgumentException($"Script {name} not found. Reloading is only possible for previously loaded scripts", nameof(name));

        var metadata = script.MetaData;
        var source = metadata.Source;
        var flags = metadata.CreationFlags;

        provider.UnloadByNameAsync(name);

        return flags switch
        {
            var f when (f & LuaScriptMetaData.ECreationFlags.Memory) is not 0 => provider.LoadAsync(source, name, cancellationToken),
            var f when (f & LuaScriptMetaData.ECreationFlags.File) is not 0 => provider.LoadFromAsync(source, cancellationToken),
            _ => throw new InvalidOperationException($"Reloading a script of unknown source \"{source}\"({flags}) is not possible."),
        };
    }

    public static ValueTask<ILuaScript> ReloadAsync(ILuaScriptProvider provider, string code, string name, CancellationToken cancellationToken = default)
    {
        provider.UnloadByNameAsync(name);
        return provider.LoadAsync(code, name, cancellationToken);
    }

    public static ValueTask<ILuaScript> ReloadFromAsync(ILuaScriptProvider provider, string path, CancellationToken cancellationToken = default)
    {
        provider.UnloadBySourceAsync(path);
        return provider.LoadFromAsync(path, cancellationToken);
    }

    public static ValueTask<ILuaScript> ReloadFromAsync(ILuaScriptProvider provider, string path, string name, CancellationToken cancellationToken = default)
    {
        provider.UnloadByNameAsync(name);
        return provider.LoadFromAsync(path, name, cancellationToken);
    }
}
