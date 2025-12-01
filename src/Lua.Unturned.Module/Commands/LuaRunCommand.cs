using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Logging.Extensions;
using SDG.Unturned;
using Steamworks;
using System;
using System.Threading.Tasks;

namespace Lua.Unturned.Module.Commands;

public sealed class LuaRunCommand : Command
{
    private readonly ILuaStringExecutor m_Executer;
    private readonly ILuaLogger m_Logger;

    public LuaRunCommand(ILuaStringExecutor executor, ILuaLogger logger)
    {
        m_Executer = executor;
        m_Logger = logger;

        _command = "lua";
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
        base.execute(executorID, parameter);
        Task.Run(() => RunScriptAsync(executorID, parameter));
    }

    private async Task RunScriptAsync(CSteamID executorId, string parameter)
    {
        var logger = m_Logger;

        logger.LogInfoFormat("{0} execute script {1}", executorId.m_SteamID, parameter);

        try
        {
            var executeOutput = await m_Executer.ExecuteAsync(parameter);
            if (executeOutput is { Length: > 0 }) m_Logger.LogInfoFormat("Output:\n{0}0", string.Join('\n', executeOutput));
        }
        catch (Exception e)
        {
            logger.LogFatal(e);
        }
    }
}
