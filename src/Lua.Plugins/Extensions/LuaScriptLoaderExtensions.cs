using Lua.Scripting.Abstraction;
using Lua.Scripting.Extensions;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Logging.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Plugins.Extensions;

public static class LuaScriptLoaderExtensions
{
    public static async Task TryLoadFromAndExecuteWithLoggingAsync(this ILuaScriptLoader loader, string path, ILuaLogger logger, CancellationToken cancellationToken = default)
    {
        try
        {
            var script = await loader.LoadFromAsync(path, cancellationToken);
            var loadOutput = await script.ExecuteAsync();

            logger.LogInfoFormat("Lua script {0} loaded successfully.", path);
            logger.LogInfoFormat("Lua script {0} Output:\n{1}", path, string.Join("\n", loadOutput));
        }
        catch (Exception e)
        {
            logger.LogFatalFormat("An unexpected error occurred while loading the Lua script {0}.", e, path);
        }
    }
}
