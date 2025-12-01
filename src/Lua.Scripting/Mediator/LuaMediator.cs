using Lua.Internal;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator;

public sealed class LuaMediator(ILuaScriptProvider provider) : ILuaMediator
{
    private readonly ILuaScriptProvider m_Provider = provider;

    public ValueTask NotifyAsync(string callback, ReadOnlySpan<LuaValue> notificationArguments, CancellationToken cancellationToken = default)
    {
        var pooled = new PooledArray<LuaValue>(notificationArguments.Length);
        notificationArguments.CopyTo(pooled.AsSpan());
        return InternalNotifyAsync(callback, pooled.AsMemory(), cancellationToken);
    }

    public ValueTask<LuaValue[]> RequestAsync(string callback, ReadOnlySpan<LuaValue> requestArguments, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;
            return script.CallValueAsync(handler, requestArguments, cancellationToken);
        }

        return new([]);
    }

    private async ValueTask InternalNotifyAsync(string callback, ReadOnlyMemory<LuaValue> notificationArguments, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;
            _ = await script.CallValueAsync(handler, notificationArguments.Span, cancellationToken);
        }
    }
}
