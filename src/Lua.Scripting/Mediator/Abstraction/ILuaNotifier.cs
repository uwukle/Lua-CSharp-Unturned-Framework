using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator.Abstraction;

public interface ILuaNotifier
{
    ValueTask NotifyAsync(string callback, LuaValue notification, CancellationToken cancellationToken = default);
}
