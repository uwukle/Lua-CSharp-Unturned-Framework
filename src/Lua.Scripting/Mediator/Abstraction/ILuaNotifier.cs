using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator.Abstraction;

public interface ILuaNotifier
{
    ValueTask NotifyAsync(string callback, ReadOnlySpan<LuaValue> notificationArguments, CancellationToken cancellationToken = default);
}
