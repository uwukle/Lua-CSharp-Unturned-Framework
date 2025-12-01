using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins.Abstraction;

public interface ILuaPlugin
{
    string Name { get; }

    ILuaScriptProvider ScriptProvider { get; }

    ILuaModuleLoaderProvider ModuleLoaderProvider { get; }

    ILuaLogger Logger { get; }

    ILuaMediator Mediator { get; }

    IEnumerable<ILuaModuleLoader> Loaders { get; }

    IEnumerable<LuaRequiredScript> RequiredScripts { get; }

    ValueTask LoadAsync(CancellationToken cancellationToken = default);

    ValueTask UnloadAsync(CancellationToken cancellationToken = default);
}
