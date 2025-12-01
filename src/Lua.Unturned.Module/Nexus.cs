using Lua.Plugins;
using Lua.Scripting;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Extensions;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Logging.Extensions;
using Lua.Scripting.Mediator;
using Lua.Scripting.Mediator.Abstraction;
using Lua.Unturned.Module.Extensions;
using SDG.Framework.Modules;
using SDG.Unturned;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Unturned.Module;

public sealed class Nexus : IModuleNexus
{
    private sealed class DirectoryBinder(string bindDirectory) : IDisposable
    {
        private readonly string m_BindDirectory = bindDirectory;
        private string? m_OriginalDirectory;

        internal bool IsBinded { get; private set; }

        internal string? Current => IsBinded ? m_BindDirectory : m_OriginalDirectory;

        internal void Bind()
        {
            if (IsBinded) return;

            m_OriginalDirectory = Directory.GetCurrentDirectory();

            if (SetDirectory(m_BindDirectory)) return;

            IsBinded = true;
        }

        public void Unbind()
        {
            if (!IsBinded) return;
            if (!SetDirectory(m_OriginalDirectory)) return;

            IsBinded = false;
        }

        private bool SetDirectory(string? path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            Directory.CreateDirectoryIfNeeded(path);
            Directory.SetCurrentDirectory(path);

            return true;
        }

        public void Dispose() => Unbind();
    }

    private const string LUA_STARTUP_DIRECTORY = "Startup";
    private const string LUA_PLUGINS_DIRECTORY = "Plugins";

    private static Nexus? m_Instance;

    private readonly CancellationTokenSource m_CancellationTokenSource;

    private readonly LuaScriptProvider m_ScriptProvider;
    private readonly LuaMediator m_Mediator;
    private readonly LuaUnturnedLogger m_Logger;
    private readonly LuaPluginProvider m_PluginProvider;

    private readonly DirectoryBinder m_DirectoryBinder;

    public Nexus()
    {
        m_CancellationTokenSource = new();

        m_ScriptProvider = new(LuaUnturnedPlatform.Default);
        m_Logger = new();
        m_Mediator = new(m_Logger, m_ScriptProvider);
        m_PluginProvider = new(m_ScriptProvider, m_ScriptProvider, m_Logger, m_Mediator);

        m_DirectoryBinder = new(Path.Combine("Servers", Dedicator.serverID, "Lua"));
    }

    public static Nexus Instance => m_Instance ?? throw new NullReferenceException($"Not a single instance of the {nameof(Nexus)} was ever initialized.");

    public ILuaScriptProvider ScriptProvider => m_ScriptProvider;

    public ILuaMediator Mediator => m_Mediator;

    public ILuaLogger Logger => m_Logger;

    void IModuleNexus.initialize()
    {
        if (m_Instance is not null) throw new InvalidOperationException($"It is not possible to initialize more than one {nameof(Nexus)} instance.");

        m_Instance = this;

        m_DirectoryBinder.Bind();

        Task.Run(() => LoadNexusAsync(m_CancellationTokenSource.Token));
    }

    void IModuleNexus.shutdown()
    {
        if (m_Instance is null) throw new InvalidOperationException($"It is not possible to shutdown {nameof(Nexus)} instance without initialize.");

        m_Instance = null;
        m_DirectoryBinder.Dispose();

        m_CancellationTokenSource.Cancel();
        Task.Run(m_ScriptProvider.DisposeAsync);
    }

    private async Task LoadNexusAsync(CancellationToken cancellationToken = default)
    {
        await LoadPluginsAsync(cancellationToken);
        await LoadDefaultScriptsAsync(cancellationToken);
        await m_Mediator.OnNexusLoadedAsync(cancellationToken);
    }

    private async Task LoadDefaultScriptsAsync(CancellationToken cancellationToken = default)
    {
        var logger = m_Logger;
        var provider = m_ScriptProvider;
        var mediator = m_Mediator;

        Directory.CreateDirectoryIfNeeded(LUA_STARTUP_DIRECTORY);

        foreach (var scriptPath in Directory.GetFiles(LUA_STARTUP_DIRECTORY, "*.lua", SearchOption.TopDirectoryOnly))
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var script = await provider.LoadFromAsync(scriptPath, cancellationToken);
                var loadOutput = await script.ExecuteAsync();

                logger.LogInfoFormat("Lua script {0} loaded successfully.", scriptPath);
                logger.LogInfoFormat("Lua script {0} Output:\n{1}", scriptPath, string.Join("\n", loadOutput));

                await mediator.OnLoadedAsync(loadOutput, cancellationToken);
                await mediator.OnScriptLoadedAsync(script, loadOutput, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogFatalFormat("An unexpected error occurred while loading the Lua script {0}.", e, scriptPath);
            }
        }
    }

    private async Task LoadPluginsAsync(CancellationToken cancellationToken = default)
    {
        var logger = m_Logger;
        var provider = m_PluginProvider;

        Directory.CreateDirectoryIfNeeded(LUA_PLUGINS_DIRECTORY);

        foreach (var pluginPath in Directory.GetFiles(LUA_PLUGINS_DIRECTORY, "*.dll", SearchOption.TopDirectoryOnly))
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await provider.LoadFromAsync(pluginPath, cancellationToken);
                logger.LogInfoFormat("Plugins dll {0} loaded successfully.", pluginPath);
            }
            catch (Exception e)
            {
                logger.LogFatalFormat("An unexpected error occurred while loading the plugins dll {0}.", e, pluginPath);
            }
        }
    }
}
