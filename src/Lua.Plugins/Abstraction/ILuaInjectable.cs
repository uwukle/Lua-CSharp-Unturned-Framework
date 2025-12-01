using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Mediator.Abstraction;

namespace Lua.Plugins.Abstraction;

public interface ILuaInjectable 
{
    void Inject(ILuaScriptProvider scriptProvider, ILuaModuleLoaderProvider moduleLoaderProvider, ILuaLogger logger, ILuaMediator mediator);
}
