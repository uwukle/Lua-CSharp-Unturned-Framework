using Lua.Scripting.Abstraction;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Extensions;

public static class LuaScriptLoaderExtensions
{
    public static ValueTask<ILuaScript> LoadFromAsync(this ILuaScriptLoader loader, string path, CancellationToken cancellationToken = default) => loader.LoadFromAsync(path, Path.GetFileNameWithoutExtension(path), cancellationToken);
}
