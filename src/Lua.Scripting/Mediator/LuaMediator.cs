using Lua.Scripting.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System.Threading;
using System.Threading.Tasks;

namespace Lua.Scripting.Mediator;

public sealed class LuaMediator(ILuaScriptProvider provider) : ILuaMediator
{
    private readonly ILuaScriptProvider m_Provider = provider;

    public async ValueTask NotifyAsync(string callback, LuaValue notification, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;
            _ = await script.CallValueAsync(handler, [notification], cancellationToken);
        }
    }

    public async ValueTask<LuaValue[]> RequestAsync(string callback, LuaValue request, CancellationToken cancellationToken = default)
    {
        foreach (var script in m_Provider.Scripts)
        {
            if (!script.TryGetValue(callback, out var handler)) continue;
            return await script.CallValueAsync(handler, [request], cancellationToken);
        }

        return [];
    }
}
