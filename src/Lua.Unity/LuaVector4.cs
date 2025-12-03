using Lua.Scripting;
using Lua.Scripting.Abstraction;
using System;
using UnityEngine;

namespace Lua.Unity;

[LuaObject(nameof(Vector4))]
public sealed partial class LuaVector4(float x, float y, float z, float w) : ILuaNormalizable, ILuaCopiable<LuaVector4>, IEquatable<LuaVector4>, IEquatable<Vector4>
{
    [LuaIgnoreMember]
    private Vector4 m_Source = new(x, y, z, w);

    public LuaVector4(Vector2 vector2) : this(vector2.x, vector2.y, 0.0f, 0.0f) { }

    public LuaVector4(Vector3 vector3) : this(vector3.x, vector3.y, vector3.z, 0.0f) { }

    public LuaVector4(Vector4 vector4) : this(vector4.x, vector4.y, vector4.z, vector4.w) { }

    public LuaVector4(Rect rect) : this(rect.x, rect.y, rect.width, rect.height) { }

    public LuaVector4(Quaternion quaternion) : this(quaternion.x, quaternion.y, quaternion.z, quaternion.w) { }

    public LuaVector4(LuaVector2 vector2) : this(vector2.X, vector2.Y, 0.0f, 0.0f) { }

    public LuaVector4(LuaVector3 vector3) : this(vector3.X, vector3.Y, vector3.Z, 0.0f) { }

    public LuaVector4(LuaVector4 vector4) : this(vector4.X, vector4.Y, vector4.Z, vector4.W) { }

    public LuaVector4() : this(0.0f, 0.0f, 0.0f, 0.0f) { }

    [LuaMember(nameof(Vector4.zero))]
    public static LuaVector4 Zero => new(Vector4.zero);

    [LuaMember(nameof(Vector4.one))]
    public static LuaVector4 One => new(Vector4.one);

    [LuaMember(nameof(Vector4.positiveInfinity))]
    public static LuaVector4 PositiveInfinity => new(Vector4.positiveInfinity);

    [LuaMember(nameof(Vector4.negativeInfinity))]
    public static LuaVector4 NegativeInfinity => new(Vector4.negativeInfinity);

    [LuaMember(nameof(Vector4.x))]
    public float X
    {
        get => m_Source.x;
        set => m_Source.x = value;
    }

    [LuaMember(nameof(Vector4.y))]
    public float Y
    {
        get => m_Source.y;
        set => m_Source.y = value;
    }

    [LuaMember(nameof(Vector4.z))]
    public float Z
    {
        get => m_Source.z;
        set => m_Source.z = value;
    }

    [LuaMember(nameof(Vector4.w))]
    public float W
    {
        get => m_Source.w;
        set => m_Source.w = value;
    }

    [LuaMember(nameof(Vector4.normalized))]
    public LuaVector4 Normalized => new(m_Source.normalized);

    [LuaMember(nameof(Vector4.magnitude))]
    public float Magnitude => m_Source.magnitude;

    [LuaMember(nameof(Vector4.sqrMagnitude))]
    public float SqrMagnitude => m_Source.sqrMagnitude;

    [LuaMember(LuaNamingStandard.CONSTRUCTOR_FUNCTION_NAME)]
    public static LuaVector4 New(float x, float y, float z, float w) => new(x, y, z, w);

    [LuaMember(nameof(Vector4.Set))]
    public void Set(float newX, float newY, float newZ, float newW) => m_Source.Set(newX, newY, newZ, newW);

    [LuaIgnoreMember]
    public void Set(Vector4 vector4) => m_Source = vector4;

    [LuaIgnoreMember]
    public void Set(LuaVector4 vector4) => Set(vector4.ToUnityVector4());

    [LuaMember(nameof(ScaleInPlace))]
    public void ScaleInPlace(LuaVector4 vector4)
    {
        X *= vector4.X;
        Y *= vector4.Y;
        Z *= vector4.Z;
        W *= vector4.W;
    }

    [LuaMember(nameof(Vector4.Normalize))]
    public void Normalize() => m_Source.Normalize();

    [LuaIgnoreMember]
    public bool Equals(LuaVector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    [LuaIgnoreMember]
    public bool Equals(Vector4 other) => X == other.x && Y == other.y && Z == other.z && W == other.w;

    [LuaMember(nameof(Copy))]
    public LuaVector4 Copy() => new(this);

    [LuaIgnoreMember]
    public override bool Equals(object obj) => obj switch
    {
        Vector4 vector4 => Equals(vector4),
        LuaVector4 vector4 => Equals(vector4),
        _ => false,
    };

    [LuaIgnoreMember]
    public override int GetHashCode() => m_Source.GetHashCode();

    [LuaIgnoreMember]
    public override string ToString() => m_Source.ToString();

    [LuaMember(nameof(AddInPlace))]
    public void AddInPlace(LuaVector4 b) => Set(ToUnityVector4() + b.ToUnityVector4());

    [LuaMember(nameof(SubtractInPlace))]
    public void SubtractInPlace(LuaVector4 b) => Set(ToUnityVector4() - b.ToUnityVector4());

    [LuaMember(nameof(NegateInPlace))]
    public void NegateInPlace() => Set(-ToUnityVector4());

    [LuaMember(nameof(MinInPlace))]
    public void MinInPlace(LuaVector4 rhs) => Set(Vector4.Min(ToUnityVector4(), rhs.ToUnityVector4()));

    [LuaMember(nameof(MaxInPlace))]
    public void MaxInPlace(LuaVector4 rhs) => Set(Vector4.Max(ToUnityVector4(), rhs.ToUnityVector4()));

    [LuaMember(nameof(LerpInPlace))]
    public void LerpInPlace(LuaVector4 b, float t) => Set(Vector4.Lerp(ToUnityVector4(), b.ToUnityVector4(), t));

    [LuaMember(nameof(LerpUnclampedInPlace))]
    public void LerpUnclampedInPlace(LuaVector4 b, float t) => Set(Vector4.LerpUnclamped(ToUnityVector4(), b.ToUnityVector4(), t));

    [LuaMember(nameof(MoveTowardsInPlace))]
    public void MoveTowardsInPlace(LuaVector4 target, float maxDistanceDelta) => Set(Vector4.MoveTowards(ToUnityVector4(), target.ToUnityVector4(), maxDistanceDelta));

    [LuaMetamethod(LuaObjectMetamethod.Add)]
    public static LuaVector4 Add(LuaVector4 a, LuaVector4 b) => new(a.ToUnityVector4() + b.ToUnityVector4());

    [LuaMetamethod(LuaObjectMetamethod.Sub)]
    public static LuaVector4 Subtract(LuaVector4 a, LuaVector4 b) => new(a.ToUnityVector4() - b.ToUnityVector4());

    [LuaMetamethod(LuaObjectMetamethod.Unm)]
    public static LuaVector4 Negate(LuaVector4 a) => new(-a.ToUnityVector4());

    [LuaMetamethod(LuaObjectMetamethod.Eq)]
    public static bool Equals(LuaVector4 lhs, LuaVector4 rhs) => lhs.ToUnityVector4() == rhs.ToUnityVector4();

    [LuaMember(nameof(Vector4.Dot))]
    public static float Dot(LuaVector4 lhs, LuaVector4 rhs) => Vector4.Dot(lhs.ToUnityVector4(), rhs.ToUnityVector4());

    [LuaMember(nameof(Vector4.Distance))]
    public static float Distance(LuaVector4 a, LuaVector4 b) => Vector4.Distance(a.ToUnityVector4(), b.ToUnityVector4());

    [LuaMember(nameof(Vector4.Min))]
    public static LuaVector4 Min(LuaVector4 lhs, LuaVector4 rhs) => new(Vector4.Min(lhs.ToUnityVector4(), rhs.ToUnityVector4()));

    [LuaMember(nameof(Vector4.Max))]
    public static LuaVector4 Max(LuaVector4 lhs, LuaVector4 rhs) => new(Vector4.Max(lhs.ToUnityVector4(), rhs.ToUnityVector4()));

    [LuaMember(nameof(Vector4.Lerp))]
    public static LuaVector4 Lerp(LuaVector4 a, LuaVector4 b, float t) => new(Vector4.Lerp(a.ToUnityVector4(), b.ToUnityVector4(), t));

    [LuaMember(nameof(Vector4.LerpUnclamped))]
    public static LuaVector4 LerpUnclamped(LuaVector4 a, LuaVector4 b, float t) => new(Vector4.LerpUnclamped(a.ToUnityVector4(), b.ToUnityVector4(), t));

    [LuaMember(nameof(Vector4.MoveTowards))]
    public static LuaVector4 MoveTowards(LuaVector4 current, LuaVector4 target, float maxDistanceDelta) => new(Vector4.MoveTowards(current.ToUnityVector4(), target.ToUnityVector4(), maxDistanceDelta));

    [LuaMember(nameof(Vector4.Scale))]
    public static LuaVector4 Scale(LuaVector4 a, LuaVector4 b) => new(Vector4.Scale(a.ToUnityVector4(), b.ToUnityVector4()));

    [LuaMember(nameof(Vector4.Project))]
    public static LuaVector4 Project(LuaVector4 vector, LuaVector4 onNormal) => new(Vector4.Project(vector.ToUnityVector4(), onNormal.ToUnityVector4()));

    [LuaMember(nameof(ToVector2))]
    public LuaVector2 ToVector2() => new(X, Y);

    [LuaMember(nameof(ToVector3))]
    public LuaVector3 ToVector3() => new(X, Y, Z);

    [LuaMember(nameof(Vector4.Magnitude))]
    public static float MagnitudeStatic(LuaVector4 vector) => vector.Magnitude;

    [LuaMember(nameof(Vector4.SqrMagnitude))]
    public static float SqrMagnitudeStatic(LuaVector4 vector) => vector.SqrMagnitude;

    [LuaMember(nameof(GetHomogeneousPoint))]
    public LuaVector3 GetHomogeneousPoint() => Mathf.Approximately(W, 0.0f) ? new(X, Y, Z) : new(X / W, Y / W, Z / W);

    [LuaMember(nameof(GetHomogeneousVector))]
    public LuaVector3 GetHomogeneousVector() => new(X, Y, Z);

    [LuaIgnoreMember]
    public Vector4 ToUnityVector4() => new(X, Y, Z, W);

    ILuaUserData ILuaCopiable.Copy() => Copy();
}