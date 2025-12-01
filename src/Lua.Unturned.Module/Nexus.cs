using Lua.Scripting;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Extensions;
using Lua.Scripting.Logging.Extensions;
using Lua.Scripting.Mediator;
using Lua.Scripting.Mediator.Abstraction;
using SDG.Framework.Modules;
using SDG.Unturned;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Unturned.Module;

public class Nexus : IModuleNexus
{
    private sealed class DirectoryBinder(string bindDirectory)
    {
        private readonly string m_BindDirectory = bindDirectory;
        private string? m_OriginalDirectory;

        internal bool IsBinded { get; private set; }

        internal string? Current => IsBinded ? m_BindDirectory : m_OriginalDirectory;

        internal void Bind()
        {
            m_OriginalDirectory = Directory.GetCurrentDirectory();
            if (SetDirectory(m_BindDirectory)) return;
            IsBinded = true;
        }

        public void Unbind()
        {
            if (!SetDirectory(m_OriginalDirectory)) return;
            IsBinded = false;
        }

        private bool SetDirectory(string? path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Directory.SetCurrentDirectory(path);
            return true;
        }
    }

    private const string LUA_STARTUP_DIRECTORY = "Startup";

    private static Nexus? m_Instance;
    private readonly CancellationTokenSource m_CancellationTokenSource = new();
    private readonly LuaScriptProvider m_Provider;
    private readonly LuaMediator m_Mediator;
    private readonly LuaUnturnedLogger m_Logger;
    private readonly DirectoryBinder m_DirectoryBinder;

    public Nexus()
    {
        m_Provider = new(LuaUnturnedPlatform.Default);
        m_Mediator = new(m_Provider);
        m_Logger = new();
        m_DirectoryBinder = new(Path.Combine("Servers", Dedicator.serverID, "Lua"));
    }

    public static Nexus Instance => m_Instance ?? throw new NullReferenceException($"Not a single instance of the {nameof(Nexus)} was ever initialized.");

    public ILuaScriptProvider Provider => m_Provider;

    public ILuaMediator Mediator => m_Mediator;

    void IModuleNexus.initialize()
    {
        if (m_Instance is not null) throw new InvalidOperationException($"It is not possible to initialize more than one {nameof(Nexus)} instance.");

        m_Instance = this;
        m_DirectoryBinder.Bind();

        m_CancellationTokenSource.Cancel();
        Task.Run(() => LoadDefaultScriptsAsync(m_CancellationTokenSource.Token));
    }

    void IModuleNexus.shutdown()
    {
        m_Instance = null;
        m_DirectoryBinder.Unbind();

        m_CancellationTokenSource.Cancel();
        Task.Run(() => m_Provider.DisposeAsync(m_CancellationTokenSource.Token));
    }

    private async Task LoadDefaultScriptsAsync(CancellationToken cancellationToken = default)
    {
        var logger = m_Logger;
        var provider = m_Provider;

        foreach (var scriptPath in Directory.GetFiles(LUA_STARTUP_DIRECTORY, "*.lua", SearchOption.TopDirectoryOnly))
        {
            try
            {
                var script = await provider.LoadFromAsync(scriptPath, cancellationToken);
                var loadResult = await script.ExecuteAsync();

                logger.LogInfoFormat("Lua script {0} loaded successfully.", scriptPath);
                logger.LogInfoFormat("Lua script {0} Output:\n{1}", scriptPath, string.Join("\n", loadResult));
            }
            catch (Exception e)
            {
                logger.LogFatalFormat("An unexpected error occurred while loading the Lua script {0}.", e, scriptPath);
            }
        }
    }
}
