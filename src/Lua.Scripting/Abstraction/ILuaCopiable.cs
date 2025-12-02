namespace Lua.Scripting.Abstraction;

public interface ILuaCopiable<out TUserData> : ILuaCopiable where TUserData : ILuaCopiable<TUserData>, ILuaUserData
{
    new TUserData Copy();
}

public interface ILuaCopiable
{
    ILuaUserData Copy();
}
