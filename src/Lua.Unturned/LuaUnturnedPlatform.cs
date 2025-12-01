using Lua.IO;
using Lua.Platforms;
using Lua.Unity;
using System;

namespace Lua.Unturned;

public sealed record LuaUnturnedPlatform(ILuaFileSystem FileSystem, ILuaOsEnvironment OsEnvironment, ILuaStandardIO StandardIO, TimeProvider TimeProvider) : LuaPlatform(FileSystem, OsEnvironment, StandardIO, TimeProvider)
{
    public static new LuaUnturnedPlatform Default => field ??= new(new FileSystem(), new LuaUnityEnvironment(), new ConsoleStandardIO(), TimeProvider.System);
}
