using Lua.Scripting;
using Lua.Scripting.Abstraction;
using System;
using UnityEngine;

namespace Lua.Unity;

[LuaObject(nameof(Quaternion))]
public sealed partial class LuaQuaternion(float x, float y, float z, float w) : ILuaNormalizable, ILuaCopiable<LuaQuaternion>, IEquatable<LuaQuaternion>, IEquatable<Quaternion>
{
    [LuaIgnoreMember]
    private Quaternion m_Source = new(x, y, z, w);

    public LuaQuaternion(Quaternion quaternion) : this(quaternion.x, quaternion.y, quaternion.z, quaternion.w) { }

    public LuaQuaternion(Vector3 eulerAngles) : this(Quaternion.Euler(eulerAngles)) { }

    public LuaQuaternion(Vector4 vector4) : this(vector4.x, vector4.y, vector4.z, vector4.w) { }

    public LuaQuaternion(LuaVector3 eulerAngles) : this(Quaternion.Euler(eulerAngles.ToUnityVector3())) { }

    public LuaQuaternion(LuaVector4 vector4) : this(vector4.X, vector4.Y, vector4.Z, vector4.W) { }

    public LuaQuaternion(LuaQuaternion quaternion) : this(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W) { }

    public LuaQuaternion() : this(0.0f, 0.0f, 0.0f, 1.0f) { }

    [LuaMember(nameof(Quaternion.identity))]
    public static LuaQuaternion Identity => new(Quaternion.identity);

    [LuaMember(nameof(Quaternion.x))]
    public float X
    {
        get => m_Source.x;
        set => m_Source.x = value;
    }

    [LuaMember(nameof(Quaternion.y))]
    public float Y
    {
        get => m_Source.y;
        set => m_Source.y = value;
    }

    [LuaMember(nameof(Quaternion.z))]
    public float Z
    {
        get => m_Source.z;
        set => m_Source.z = value;
    }

    [LuaMember(nameof(Quaternion.w))]
    public float W
    {
        get => m_Source.w;
        set => m_Source.w = value;
    }

    [LuaMember(nameof(Quaternion.eulerAngles))]
    public LuaVector3 EulerAngles => new(m_Source.eulerAngles);

    [LuaMember(nameof(Quaternion.normalized))]
    public LuaQuaternion Normalized => new(m_Source.normalized);

    [LuaMember(LuaNamingStandard.CONSTRUCTOR_FUNCTION_NAME)]
    public static LuaQuaternion New(float x, float y, float z, float w) => new(x, y, z, w);

    [LuaMember(nameof(SetFromEulerAnglesInPlace))]
    public void SetFromEulerAnglesInPlace(LuaVector3 eulerAngles) => m_Source = Quaternion.Euler(eulerAngles.ToUnityVector3());

    [LuaMember(nameof(SetFromAxisAngleInPlace))]
    public void SetFromAxisAngleInPlace(LuaVector3 axis, float angle) => m_Source = Quaternion.AngleAxis(angle, axis.ToUnityVector3());

    [LuaMember(nameof(SetLookRotationInPlace))]
    public void SetLookRotationInPlace(LuaVector3 forward, LuaVector3 upwards) => m_Source = Quaternion.LookRotation(forward.ToUnityVector3(), upwards.ToUnityVector3());

    [LuaMember(nameof(SetLookRotationForwardInPlace))]
    public void SetLookRotationForwardInPlace(LuaVector3 forward) => m_Source = Quaternion.LookRotation(forward.ToUnityVector3());

    [LuaIgnoreMember]
    public void Set(Quaternion quaternion) => m_Source = quaternion;

    [LuaIgnoreMember]
    public void Set(LuaQuaternion quaternion) => Set(quaternion.ToUnityQuaternion());

    [LuaMember(nameof(Quaternion.Normalize))]
    public void Normalize() => m_Source.Normalize();

    [LuaIgnoreMember]
    public bool Equals(LuaQuaternion other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

    [LuaIgnoreMember]
    public bool Equals(Quaternion other) => X == other.x && Y == other.y && Z == other.z && W == other.w;

    [LuaMember(nameof(Copy))]
    public LuaQuaternion Copy() => new(this);

    [LuaIgnoreMember]
    public override bool Equals(object obj) => obj switch
    {
        Quaternion quaternion => Equals(quaternion),
        LuaQuaternion quaternion => Equals(quaternion),
        _ => false,
    };

    [LuaIgnoreMember]
    public override int GetHashCode() => m_Source.GetHashCode();

    [LuaIgnoreMember]
    public override string ToString() => m_Source.ToString();

    // InPlace операции
    [LuaMember(nameof(AddInPlace))]
    public void AddInPlace(LuaQuaternion b) => Set(ToUnityQuaternion() * b.ToUnityQuaternion());

    [LuaMember(nameof(SubtractInPlace))]
    public void SubtractInPlace(LuaQuaternion b) => Set(ToUnityQuaternion() * Quaternion.Inverse(b.ToUnityQuaternion()));

    [LuaMember(nameof(NegateInPlace))]
    public void NegateInPlace() => Set(new Quaternion(-X, -Y, -Z, -W));

    [LuaMember(nameof(ScaleInPlace))]
    public void ScaleInPlace(float scale)
    {
        X *= scale;
        Y *= scale;
        Z *= scale;
        W *= scale;
    }

    [LuaMember(nameof(InvertInPlace))]
    public void InvertInPlace() => Set(Quaternion.Inverse(ToUnityQuaternion()));

    [LuaMember(nameof(RotateTowardsInPlace))]
    public void RotateTowardsInPlace(LuaQuaternion target, float maxDegreesDelta) =>
        Set(Quaternion.RotateTowards(ToUnityQuaternion(), target.ToUnityQuaternion(), maxDegreesDelta));

    [LuaMember(nameof(SlerpInPlace))]
    public void SlerpInPlace(LuaQuaternion b, float t) => Set(Quaternion.Slerp(ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(SlerpUnclampedInPlace))]
    public void SlerpUnclampedInPlace(LuaQuaternion b, float t) => Set(Quaternion.SlerpUnclamped(ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(LerpInPlace))]
    public void LerpInPlace(LuaQuaternion b, float t) => Set(Quaternion.Lerp(ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(LerpUnclampedInPlace))]
    public void LerpUnclampedInPlace(LuaQuaternion b, float t) => Set(Quaternion.LerpUnclamped(ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMetamethod(LuaObjectMetamethod.Add)]
    public static LuaQuaternion Add(LuaQuaternion a, LuaQuaternion b) => new(a.ToUnityQuaternion() * b.ToUnityQuaternion());

    [LuaMetamethod(LuaObjectMetamethod.Sub)]
    public static LuaQuaternion Subtract(LuaQuaternion a, LuaQuaternion b) => new(a.ToUnityQuaternion() * Quaternion.Inverse(b.ToUnityQuaternion()));

    [LuaMetamethod(LuaObjectMetamethod.Unm)]
    public static LuaQuaternion Negate(LuaQuaternion a) => new(-a.X, -a.Y, -a.Z, -a.W);

    [LuaMetamethod(LuaObjectMetamethod.Eq)]
    public static bool Equals(LuaQuaternion lhs, LuaQuaternion rhs) => lhs.ToUnityQuaternion() == rhs.ToUnityQuaternion();

    [LuaMember(nameof(Quaternion.Euler))]
    public static LuaQuaternion Euler(float x, float y, float z) => new(Quaternion.Euler(x, y, z));

    [LuaMember(nameof(Quaternion.AngleAxis))]
    public static LuaQuaternion AngleAxis(float angle, LuaVector3 axis) => new(Quaternion.AngleAxis(angle, axis.ToUnityVector3()));

    [LuaMember(nameof(Quaternion.LookRotation))]
    public static LuaQuaternion LookRotation(LuaVector3 forward, LuaVector3 upwards) => new(Quaternion.LookRotation(forward.ToUnityVector3(), upwards.ToUnityVector3()));

    [LuaMember(nameof(Quaternion.FromToRotation))]
    public static LuaQuaternion FromToRotation(LuaVector3 fromDirection, LuaVector3 toDirection) => new(Quaternion.FromToRotation(fromDirection.ToUnityVector3(), toDirection.ToUnityVector3()));

    [LuaMember(nameof(Quaternion.Dot))]
    public static float Dot(LuaQuaternion a, LuaQuaternion b) => Quaternion.Dot(a.ToUnityQuaternion(), b.ToUnityQuaternion());

    [LuaMember(nameof(Quaternion.Angle))]
    public static float Angle(LuaQuaternion a, LuaQuaternion b) => Quaternion.Angle(a.ToUnityQuaternion(), b.ToUnityQuaternion());

    [LuaMember(nameof(Quaternion.Inverse))]
    public static LuaQuaternion Inverse(LuaQuaternion rotation) => new(Quaternion.Inverse(rotation.ToUnityQuaternion()));

    [LuaMember(nameof(Quaternion.Slerp))]
    public static LuaQuaternion Slerp(LuaQuaternion a, LuaQuaternion b, float t) => new(Quaternion.Slerp(a.ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(Quaternion.SlerpUnclamped))]
    public static LuaQuaternion SlerpUnclamped(LuaQuaternion a, LuaQuaternion b, float t) => new(Quaternion.SlerpUnclamped(a.ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(Quaternion.Lerp))]
    public static LuaQuaternion Lerp(LuaQuaternion a, LuaQuaternion b, float t) => new(Quaternion.Lerp(a.ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(Quaternion.LerpUnclamped))]
    public static LuaQuaternion LerpUnclamped(LuaQuaternion a, LuaQuaternion b, float t) => new(Quaternion.LerpUnclamped(a.ToUnityQuaternion(), b.ToUnityQuaternion(), t));

    [LuaMember(nameof(Quaternion.RotateTowards))]
    public static LuaQuaternion RotateTowards(LuaQuaternion from, LuaQuaternion to, float maxDegreesDelta) => new(Quaternion.RotateTowards(from.ToUnityQuaternion(), to.ToUnityQuaternion(), maxDegreesDelta));

    [LuaMember(nameof(MultiplyVectorInPlace))]
    public LuaVector3 MultiplyVectorInPlace(LuaVector3 vector) => new(m_Source * vector.ToUnityVector3());

    [LuaMember(nameof(MultiplyPointInPlace))]
    public LuaVector3 MultiplyPointInPlace(LuaVector3 point) => new(m_Source * point.ToUnityVector3());

    [LuaMember(nameof(MultiplyVector))]
    public static LuaVector3 MultiplyVector(LuaQuaternion rotation, LuaVector3 vector) => new(rotation.ToUnityQuaternion() * vector.ToUnityVector3());

    [LuaMember(nameof(MultiplyPoint))]
    public static LuaVector3 MultiplyPoint(LuaQuaternion rotation, LuaVector3 point) => new(rotation.ToUnityQuaternion() * point.ToUnityVector3());

    [LuaMember(nameof(ToAngleAxis))]
    public float ToAngleAxis(LuaVector3 axis)
    {
        m_Source.ToAngleAxis(out var angle, out Vector3 axisVector);
        axis.Set(axisVector);
        return angle;
    }

    [LuaMember(nameof(ToVector4))]
    public LuaVector4 ToVector4() => new(X, Y, Z, W);

    [LuaMember(nameof(GetEulerAngles))]
    public LuaVector3 GetEulerAngles() => new(m_Source.eulerAngles);

    [LuaMember(nameof(SetEulerAngles))]
    public void SetEulerAngles(LuaVector3 eulerAngles) => m_Source = Quaternion.Euler(eulerAngles.ToUnityVector3());

    [LuaIgnoreMember]
    public Quaternion ToUnityQuaternion() => new(X, Y, Z, W);

    ILuaUserData ILuaCopiable.Copy() => Copy();
}