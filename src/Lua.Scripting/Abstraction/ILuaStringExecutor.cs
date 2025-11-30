using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Abstraction;

public interface ILuaStringExecutor
{
    ValueTask<LuaValue[]> Execute(string code, CancellationToken cancellationToken = default);
}
