using Lua.Internal;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Unturned.Module.Extensions;

public static class MediatorExtensions
{
    public static ValueTask OnScriptLoadedAsync(this ILuaMediator mediator, ILuaScript script, LuaValue[] loadOutput, CancellationToken cancellationToken = default)
    {
        using var arguments = new PooledArray<LuaValue>(loadOutput.Length + 1);
        var argumentsSpan = arguments.AsSpan();
        arguments[0] = LuaValue.FromUserData(script);
        loadOutput.CopyTo(argumentsSpan[1..]);

        return mediator.NotifyAsync("OnScriptLoaded", argumentsSpan, cancellationToken);
    }

    public static ValueTask OnNexusLoadedAsync(this ILuaMediator mediator, CancellationToken cancellationToken = default) => mediator.NotifyAsync("OnNexusLoaded", [], cancellationToken);
}
