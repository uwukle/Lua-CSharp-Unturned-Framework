using Lua.Scripting.Abstraction;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Extensions;

public static class LuaScriptLoaderExtensions
{
    public static ValueTask<ILuaScript> LoadAsync(this ILuaScriptLoader loader, string code, string name, CancellationToken cancellationToken = default) => loader.LoadAsync(code, name, [], cancellationToken);

    public static ValueTask<ILuaScript> LoadFromAsync(this ILuaScriptLoader loader, string path, string name, CancellationToken cancellationToken = default) => loader.LoadFromAsync(path, name, [], cancellationToken);

    public static ValueTask<ILuaScript> LoadFromAsync(this ILuaScriptLoader loader, string path, LuaTable? environment = null, CancellationToken cancellationToken = default) => loader.LoadFromAsync(path, Path.GetFileNameWithoutExtension(path), environment, cancellationToken);

    public static ValueTask<ILuaScript> LoadFromAsync(this ILuaScriptLoader loader, string path, CancellationToken cancellationToken = default) => loader.LoadFromAsync(path, [], cancellationToken);
}
