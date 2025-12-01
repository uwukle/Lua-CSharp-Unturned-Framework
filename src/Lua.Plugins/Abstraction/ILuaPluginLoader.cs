using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins.Abstraction;

public interface ILuaPluginLoader
{
    ValueTask LoadFromAsync(string path, CancellationToken cancellationToken = default);
}
