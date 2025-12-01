using Lua.Plugins.Abstraction;
using Lua.Plugins.Tools;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Logging.Extensions;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins;

public sealed class LuaPluginProvider(ILuaScriptProvider scriptProvider, ILuaModuleLoaderProvider moduleLoaderProvider, ILuaLogger logger, ILuaMediator mediator) : ILuaPluginProvider, IAsyncDisposable
{
    private readonly ILuaScriptProvider m_ScriptProvider = scriptProvider;
    private readonly ILuaModuleLoaderProvider m_ModuleLoaderProvider = moduleLoaderProvider;
    private readonly ILuaLogger m_Logger = logger;
    private readonly ILuaMediator m_Mediator = mediator;
    private readonly List<ILuaPlugin> m_Plugins = [];

    public IEnumerable<ILuaPlugin> Plugins => m_Plugins;

    public ILuaPlugin? Get(string name) => m_Plugins.Find(p => p.Name == name);

    public bool Has(string name) => m_Plugins.FindIndex(p => p.Name == name) is not -1;

    public async ValueTask LoadFromAsync(string path, CancellationToken cancellationToken = default)
    {
        byte[] assemblyBytes = await File.ReadAllBytesAsync(path, cancellationToken);
        var assembly = Assembly.Load(assemblyBytes);

        var scriptProvider = m_ScriptProvider;
        var moduleLoaderProvider = m_ModuleLoaderProvider;
        var logger = m_Logger;
        var mediator = m_Mediator;
        var plugins = m_Plugins;

        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            if (!LuaPluginTool.IsPluginType(type)) continue;

            try
            {
                var plugin = LuaPluginTool.Create(type, scriptProvider, moduleLoaderProvider, logger, mediator);
                await plugin.LoadAsync(cancellationToken);
                plugins.Add(plugin);
            }
            catch (Exception e)
            {
                logger.LogFatalFormat("An unhandled exception occurred while loading the plugin {0}\\{1}.", e, path, type.FullName);
            }
        }
    }

    public ValueTask LoadAsync(string name, CancellationToken cancellationToken = default) => Get(name)?.LoadAsync(cancellationToken) ?? default;

    public ValueTask UnloadAsync(string name, CancellationToken cancellationToken = default) => Get(name)?.UnloadAsync(cancellationToken) ?? default;

    public async ValueTask DisposeAsync()
    {
        foreach (var plugin in m_Plugins) await plugin.UnloadAsync();
    }
}
