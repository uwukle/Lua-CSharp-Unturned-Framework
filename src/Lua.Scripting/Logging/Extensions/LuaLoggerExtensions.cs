using Lua.Scripting.Logging.Abstraction;
using System;

namespace Lua.Scripting.Logging.Extensions;

public static class LuaLoggerExtensions
{
    public static void LogFatalFormat(this ILuaLogger logger, string message, Exception exception, params object[] arguments) => logger.LogFormat($"{message} -> {exception.Message}", ILuaLogger.ELevel.Fatal, arguments);

    public static void LogFatal(this ILuaLogger logger, string message, Exception exception) => logger.Log($"{message} -> {exception.Message}", ILuaLogger.ELevel.Fatal);

    public static void LogFatalFormat(this ILuaLogger logger, Exception exception, params object[] arguments) => logger.LogFormat(exception.Message, ILuaLogger.ELevel.Fatal, arguments);

    public static void LogFatal(this ILuaLogger logger, Exception exception) => logger.Log(exception.Message, ILuaLogger.ELevel.Fatal);

    public static void LogFatalFormat(this ILuaLogger logger, string message, params object[] arguments) => logger.LogFormat(message, ILuaLogger.ELevel.Fatal, arguments);

    public static void LogFatal(this ILuaLogger logger, string message) => logger.Log(message, ILuaLogger.ELevel.Fatal);

    public static void LogInfoFormat(this ILuaLogger logger, string message, params object[] arguments) => logger.LogFormat(message, ILuaLogger.ELevel.Info, arguments);

    public static void LogInfo(this ILuaLogger logger, string message) => logger.Log(message, ILuaLogger.ELevel.Info);

    public static void LogMessageFormat(this ILuaLogger logger, string message, params object[] arguments) => logger.LogFormat(message, ILuaLogger.ELevel.Message, arguments);

    public static void LogMessage(this ILuaLogger logger, string message) => logger.Log(message, ILuaLogger.ELevel.Message);
}
