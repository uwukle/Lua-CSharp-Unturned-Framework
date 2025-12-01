using Lua.Internal;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Logging.Extensions;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator;

public sealed class LuaMediator(ILuaLogger logger, ILuaScriptProvider provider) : ILuaMediator
{
    private readonly ILuaLogger m_Logger = logger;
    private readonly ILuaScriptProvider m_Provider = provider;

    public ValueTask NotifyAsync(string callback, ReadOnlySpan<LuaValue> notificationArguments, CancellationToken cancellationToken = default)
    {
        using var pooled = new PooledArray<LuaValue>(notificationArguments.Length);
        notificationArguments.CopyTo(pooled.AsSpan());
        return InternalNotifyAsync(callback, pooled.AsMemory(), cancellationToken);
    }

    public ValueTask<LuaValue[]> RequestAsync(string callback, ReadOnlySpan<LuaValue> requestArguments, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;

            try
            {
                return script.CallValueAsync(handler, requestArguments, cancellationToken);
            }
            catch (Exception e)
            {
                m_Logger.LogFatalFormat("An unhandled exception occurred while the script {0} was processing an request {1}.", e, script.Name, callback);
                break;
            }
        }

        return new([]);
    }

    private async ValueTask InternalNotifyAsync(string callback, ReadOnlyMemory<LuaValue> notificationArguments, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;

            try
            {
                _ = await script.CallValueAsync(handler, notificationArguments.Span, cancellationToken);
            }
            catch (Exception e)
            {
                m_Logger.LogFatalFormat("An unhandled exception occurred while the script {0} was processing an notification {1}.", e, script.Name, callback);
            }
        }
    }
}
