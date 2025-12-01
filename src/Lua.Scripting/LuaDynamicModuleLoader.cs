using Lua.Scripting.Abstraction;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting;

public sealed class LuaDynamicModuleLoader : ILuaModuleLoader, ILuaModuleLoaderProvider
{
    private readonly List<ILuaModuleLoader> m_Loaders = [];

    public bool Exists(string moduleName)
    {
        foreach (var loader in m_Loaders)
        {
            if (!loader.Exists(moduleName)) continue;
            return true;
        }

        return false;
    }

    public ValueTask<LuaModule> LoadAsync(string moduleName, CancellationToken cancellationToken = default)
    {
        foreach (var loader in m_Loaders)
        {
            if (!loader.Exists(moduleName)) continue;
            return loader.LoadAsync(moduleName, cancellationToken);
        }

        throw new LuaModuleNotFoundException(moduleName);
    }

    public void Register(ILuaModuleLoader loader) => m_Loaders.Add(loader);

    public void Unregister(ILuaModuleLoader loader) => _ = m_Loaders.Remove(loader);
}