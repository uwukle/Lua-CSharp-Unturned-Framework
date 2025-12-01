using Lua.Platforms;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Lua.Unity;

public sealed class LuaUnityEnvironment : ILuaOsEnvironment
{
    public ValueTask Exit(int exitCode, CancellationToken cancellationToken)
    {
        Environment.Exit(exitCode);
        return default;
    }

    public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);

    public double GetTotalProcessorTime() => Time.time;
}
