using Lua.Plugins.Abstraction;
using Lua.Plugins.Extensions;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Extensions;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lua.Plugins;

public abstract class LuaPlugin : ILuaPlugin, ILuaInjectable
{
    private const string LOCAL_SCRIPTS_PATH = "Scripts";

    private readonly List<string> m_LoadedScriptsSources = [];

    public virtual string Name => GetType().Name;

    public string LocalPath => Path.Combine(LOCAL_SCRIPTS_PATH, Name);

#nullable disable

    public ILuaScriptProvider ScriptProvider { get; private set; }

    public ILuaModuleLoaderProvider ModuleLoaderProvider { get; private set; }

    public ILuaLogger Logger { get; private set; }

    public ILuaMediator Mediator { get; private set; }

#nullable restore

    public virtual IEnumerable<ILuaModuleLoader> Loaders => [];

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
        var logger = Logger;
        var scriptProvider = ScriptProvider;
        var scriptsSources = m_LoadedScriptsSources;
        var localPath = LocalPath;

        Directory.CreateDirectoryIfNeeded(localPath);

        foreach (var scriptPath in Directory.GetFiles(localPath, "*.lua", SearchOption.TopDirectoryOnly))
        {
            await scriptProvider.TryLoadFromAndExecuteWithLoggingAsync(scriptPath, logger, cancellationToken);
            scriptsSources.Add(scriptPath);
        }
    }

    private async Task UnloadRequiredScripts(CancellationToken cancellationToken = default)
    {
        var scriptProvider = ScriptProvider;
        var scriptsSources = m_LoadedScriptsSources;

        foreach (var source in scriptsSources) await scriptProvider.UnloadBySourceAsync(source, cancellationToken);
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
}
