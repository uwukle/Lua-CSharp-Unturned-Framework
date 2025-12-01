using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaScriptUnloader
{
    ValueTask UnloadByNameAsync(string name, CancellationToken cancellationToken = default);

    ValueTask UnloadBySourceAsync(string source, CancellationToken cancellationToken = default);
}
