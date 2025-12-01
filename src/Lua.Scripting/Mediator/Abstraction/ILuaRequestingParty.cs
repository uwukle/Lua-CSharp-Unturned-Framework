using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator.Abstraction;

public interface ILuaRequestingParty
{
    ValueTask<LuaValue[]> RequestAsync(string callback, LuaValue request, CancellationToken cancellationToken = default);
}
