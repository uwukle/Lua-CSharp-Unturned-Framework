namespace Lua.Scripting.Logging.Abstraction;

public interface ILuaLogger
{
    public enum ELevel : byte
    {
        Message,
        Info,
        Warn,
        Error,
        Exception,
        Fatal
    }

    void Log(string message, ELevel level);

    void LogFormat(string message, ELevel level, params object[] arguments);
}
