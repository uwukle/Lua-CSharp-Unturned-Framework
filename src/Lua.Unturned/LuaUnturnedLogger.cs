using Lua.Scripting.Logging.Abstraction;
using SDG.Unturned;

namespace Lua.Unturned;

public sealed class LuaUnturnedLogger : ILuaLogger
{
    public void Log(string message, ILuaLogger.ELevel level)
    {
        switch (level)
        {
            case ILuaLogger.ELevel.Warn:
                CommandWindow.LogWarning(message);
                break;
            case ILuaLogger.ELevel.Exception or ILuaLogger.ELevel.Error or ILuaLogger.ELevel.Fatal:
                CommandWindow.LogError(message);
                break;
            default:
                CommandWindow.Log(message);
                break;
        }
    }

    public void LogFormat(string message, ILuaLogger.ELevel level, params object[] arguments)
    {
        switch (level)
        {
            case ILuaLogger.ELevel.Warn:
                CommandWindow.LogWarningFormat(message, arguments);
                break;
            case ILuaLogger.ELevel.Exception or ILuaLogger.ELevel.Error or ILuaLogger.ELevel.Fatal:
                CommandWindow.LogErrorFormat(message, arguments);
                break;
            default:
                CommandWindow.LogFormat(message, arguments);
                break;
        }
    }
}
