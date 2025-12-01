namespace Lua.Scripting.Abstraction;

public interface ILuaModuleLoaderProvider
{
    void Register(ILuaModuleLoader loader);

    void Unregister(ILuaModuleLoader loader);
}
