using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptLoader
{
    ValueTask<ILuaScript> LoadAsync(string code, string name, LuaTable? environment = null, CancellationToken cancellationToken = default);

    ValueTask<ILuaScript> LoadFromAsync(string path, string name, LuaTable? environment = null, CancellationToken cancellationToken = default);
}
