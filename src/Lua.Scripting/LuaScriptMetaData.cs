using System;

namespace Lua.Scripting;

[LuaObject("ScriptMetaData")]
public sealed partial class LuaScriptMetaData(string name, string source, LuaScriptMetaData.ECreationFlags creationFlags = LuaScriptMetaData.ECreationFlags.None)
{
    [Flags]
    public enum ECreationFlags : byte
    {
        None = 0,
        CSharp = 1,
        Script = 2,
        Memory = 4,
        File = 8
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
