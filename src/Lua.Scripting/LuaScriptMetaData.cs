using System;

namespace Lua.Scripting;

[LuaObject("ScriptMetaData")]
public sealed partial class LuaScriptMetaData(string name, string source, LuaScriptMetaData.ECreationFlags creationFlags = LuaScriptMetaData.ECreationFlags.None)
{
    [Flags]
    public enum ECreationFlags : byte
    {
        None = 0,
        Memory = 1,
        File = 2,
        CSharp = 4,
        Script = 8,
    }

    [LuaMember]
    public string Name => name;

    [LuaMember]
    public string Source => source;

    [LuaIgnoreMember]
    public ECreationFlags CreationFlags => creationFlags;

    [LuaMember]
    public bool IsCreatedFromCSharp => CreationFlags.HasFlag(ECreationFlags.CSharp);

    [LuaMember]
    public bool IsCreatedFromScript => CreationFlags.HasFlag(ECreationFlags.Script);

    [LuaMember]
    public bool IsCreatedFromFile => CreationFlags.HasFlag(ECreationFlags.File);

    [LuaMember]
    public bool IsCreatedFromMemory => CreationFlags.HasFlag(ECreationFlags.Memory);
}
