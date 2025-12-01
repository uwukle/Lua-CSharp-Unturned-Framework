using Lua.Plugins.Abstraction;
using Lua.Scripting.Abstraction;
using Lua.Scripting.Logging.Abstraction;
using Lua.Scripting.Mediator.Abstraction;
using System;
using System.Linq;

namespace Lua.Plugins.Tools;

public class LuaPluginTool
{
    public static bool IsPluginType(Type type) => type.IsClass && !type.IsAbstract && type.GetConstructors().Any(c => c.GetParameters().Length == 0) && typeof(ILuaPlugin).IsAssignableFrom(type) && typeof(ILuaInjectable).IsAssignableFrom(type);

    public static ILuaPlugin Create(Type type, ILuaScriptProvider scriptProvider, ILuaModuleLoaderProvider moduleLoaderProvider, ILuaLogger logger, ILuaMediator mediator)
    {
        if (!type.IsClass) throw new ArgumentException($"The plugin type {type.FullName} must be a class.", nameof(type));
        if (type.IsAbstract) throw new ArgumentException($"The plugin type {type.FullName} must not be abstract.", nameof(type));
        if (type.GetConstructors().All(c => c.GetParameters().Length != 0)) throw new ArgumentException($"The plugin type {type.FullName} must have a parameterless constructor.", nameof(type));
        if (!typeof(ILuaPlugin).IsAssignableFrom(type)) throw new ArgumentException($"The plugin type {type.FullName} must inherit from {typeof(ILuaPlugin).FullName}", nameof(type));
        if (!typeof(ILuaInjectable).IsAssignableFrom(type)) throw new ArgumentException($"The plugin type {type.FullName} must inherit from {typeof(ILuaInjectable).FullName}", nameof(type));

        ILuaInjectable injectable = (ILuaInjectable)Activator.CreateInstance(type);
        injectable.Inject(scriptProvider, moduleLoaderProvider, logger, mediator);

#nullable disable
        return injectable as ILuaPlugin;
#nullable restore
    }

    public static TPlugin Create<TPlugin>(ILuaScriptProvider scriptProvider, ILuaModuleLoaderProvider moduleLoaderProvider, ILuaLogger logger, ILuaMediator mediator) where TPlugin : ILuaPlugin, ILuaInjectable, new()
    {
        TPlugin plugin = new();
        plugin.Inject(scriptProvider, moduleLoaderProvider, logger, mediator);
        return plugin;
    }
}
