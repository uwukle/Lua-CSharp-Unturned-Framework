using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptUnloader
{
    ValueTask UnloadAsync(string name, CancellationToken cancellationToken = default);
}
