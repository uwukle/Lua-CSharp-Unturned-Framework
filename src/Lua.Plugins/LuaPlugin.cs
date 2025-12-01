using Lua.Plugins.Abstraction;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins;

public abstract class LuaPlugin : ILuaPlugin, ILuaInjectable
{
    public virtual string Name => GetType().Name;

#nullable disable

    public ILuaScriptProvider ScriptProvider { get; private set; }

    public ILuaModuleLoaderProvider ModuleLoaderProvider { get; private set; }

    public ILuaLogger Logger { get; private set; }

    public ILuaMediator Mediator { get; private set; }

#nullable restore

    public virtual IEnumerable<ILuaModuleLoader> Loaders => [];

    public virtual IEnumerable<LuaRequiredScript> RequiredScripts => [];

    public async ValueTask LoadAsync(CancellationToken cancellationToken = default)
    {
        AttachModuleLoaders();
        await LoadRequiredScriptsAsync(cancellationToken);
        await OnLoadAsync(cancellationToken);
    }

    public async ValueTask UnloadAsync(CancellationToken cancellationToken = default)
    {
        DettachModuleLoaders();
        await UnloadRequiredScripts(cancellationToken);
        await OnUnloadAsync(cancellationToken);
    }

    protected abstract ValueTask OnLoadAsync(CancellationToken cancellationToken = default);

    protected abstract ValueTask OnUnloadAsync(CancellationToken cancellationToken = default);

    void ILuaInjectable.Inject(ILuaScriptProvider scriptProvider, ILuaModuleLoaderProvider moduleLoaderProvider, ILuaLogger logger, ILuaMediator mediator)
    {
        ScriptProvider = scriptProvider;
        ModuleLoaderProvider = moduleLoaderProvider;
        Logger = logger;
        Mediator = mediator;
    }

    private async Task LoadRequiredScriptsAsync(CancellationToken cancellationToken = default)
    {
        var scriptProvider = ScriptProvider;
        var requiredScripts = RequiredScripts;

        foreach (var requiredScript in requiredScripts)
        {
            requiredScript.Deconstruct(out var flags, out var name, out var source);

            ILuaScript script = flags switch
            {
                var f when f.HasFlag(LuaRequiredScript.EFlags.LoadFromSourceCode) => await scriptProvider.LoadAsync(source, name, cancellationToken),
                var f when f.HasFlag(LuaRequiredScript.EFlags.LoadFromFile) => await scriptProvider.LoadFromAsync(source, name, cancellationToken),
                _ => throw new InvalidOperationException($"Load a script of unknown source \"{source}\"({flags}) is not possible."),
            };

            if (flags.HasFlag(LuaRequiredScript.EFlags.ExecuteOnLoad)) await script.ExecuteAsync(cancellationToken);
        }     
    }

    private async Task UnloadRequiredScripts(CancellationToken cancellationToken = default)
    {
        var scriptProvider = ScriptProvider;
        var requiredScripts = RequiredScripts;

        foreach (var requiredScript in requiredScripts) await scriptProvider.UnloadBySourceAsync(requiredScript.Source, cancellationToken);
    }

    private void AttachModuleLoaders()
    {
        var moduleLoaderProvider = ModuleLoaderProvider;
        var loaders = Loaders;

        foreach (var loader in loaders) moduleLoaderProvider.Register(loader);
    }

    private void DettachModuleLoaders()
    {
        var moduleLoaderProvider = ModuleLoaderProvider;
        var loaders = Loaders;

        foreach (var loader in loaders) moduleLoaderProvider.Unregister(loader);
    }

    public static LuaRequiredScript RequiredSource(string name, string code, bool executeOnLoad = true) => Required(name, code, LuaRequiredScript.EFlags.LoadFromFile, executeOnLoad);

    public static LuaRequiredScript RequiredFile(string name, string path, bool executeOnLoad = true) => Required(name, path, LuaRequiredScript.EFlags.LoadFromFile, executeOnLoad);

    public static LuaRequiredScript Required(string name, string source, LuaRequiredScript.EFlags baseFlags, bool executeOnLoad = true) => Required(name, source, GetRequiredFlags(baseFlags, executeOnLoad));

    public static LuaRequiredScript Required(string name, string source, LuaRequiredScript.EFlags flags) => new(flags, name, source);

    private static LuaRequiredScript.EFlags GetRequiredFlags(LuaRequiredScript.EFlags baseFlags, bool executeOnLoad) => executeOnLoad ? baseFlags | LuaRequiredScript.EFlags.ExecuteOnLoad : baseFlags;
}
