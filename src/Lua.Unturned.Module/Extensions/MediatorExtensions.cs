using Lua.Scripting.Mediator.Abstraction;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Unturned.Module.Extensions;

public static class MediatorExtensions
{
    public static ValueTask OnNexusLoadedAsync(this ILuaMediator mediator, CancellationToken cancellationToken = default) => mediator.NotifyAsync("OnNexusLoaded", [], cancellationToken);
}
