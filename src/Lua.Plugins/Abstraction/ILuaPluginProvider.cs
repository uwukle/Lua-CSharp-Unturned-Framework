using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins.Abstraction;

public interface ILuaPluginProvider : ILuaPluginLoader
{
    IEnumerable<ILuaPlugin> Plugins { get; }

    bool Has(string name);

    ILuaPlugin? Get(string name);

    ValueTask LoadAsync(string name, CancellationToken cancellationToken = default);

    ValueTask UnloadAsync(string name, CancellationToken cancellationToken = default);
}
