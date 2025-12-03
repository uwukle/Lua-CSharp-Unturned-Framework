using Lua.Scripting;
using Lua.Scripting.Abstraction;
using System;
using UnityEngine;

namespace Lua.Unity;

[LuaObject(nameof(Vector3))]
public sealed partial class LuaVector3(float x, float y, float z) : ILuaNormalizable, ILuaCopiable<LuaVector3>, IEquatable<LuaVector3>, IEquatable<Vector3>
{
    [LuaIgnoreMember]
    private Vector3 m_Source = new(x, y, z);

    public LuaVector3(Vector2 vector2) : this(vector2.x, vector2.y, 0.0f) { }

    public LuaVector3(Vector3 vector3) : this(vector3.x, vector3.y, vector3.z) { }

    public LuaVector3(Vector4 vector4) : this(vector4.x, vector4.y, vector4.z) { }

    public LuaVector3(LuaVector2 vector2) : this(vector2.X, vector2.Y, 0.0f) { }

    public LuaVector3(LuaVector3 vector3) : this(vector3.X, vector3.Y, vector3.Z) { }

    public LuaVector3() : this(0.0f, 0.0f, 0.0f) { }

    [LuaMember(nameof(Vector3.zero))]
    public static LuaVector3 Zero => new(Vector3.zero);

    [LuaMember(nameof(Vector3.one))]
    public static LuaVector3 One => new(Vector3.one);

    [LuaMember(nameof(Vector3.up))]
    public static LuaVector3 Up => new(Vector3.up);

    [LuaMember(nameof(Vector3.down))]
    public static LuaVector3 Down => new(Vector3.down);

    [LuaMember(nameof(Vector3.left))]
    public static LuaVector3 Left => new(Vector3.left);

    [LuaMember(nameof(Vector3.right))]
    public static LuaVector3 Right => new(Vector3.right);

    [LuaMember(nameof(Vector3.forward))]
    public static LuaVector3 Forward => new(Vector3.forward);

    [LuaMember(nameof(Vector3.back))]
    public static LuaVector3 Back => new(Vector3.back);

    [LuaMember(nameof(Vector3.positiveInfinity))]
    public static LuaVector3 PositiveInfinity => new(Vector3.positiveInfinity);

    [LuaMember(nameof(Vector3.negativeInfinity))]
    public static LuaVector3 NegativeInfinity => new(Vector3.negativeInfinity);

    [LuaMember(nameof(Vector3.x))]
    public float X
    {
        get => m_Source.x;
        set => m_Source.x = value;
    }

    [LuaMember(nameof(Vector3.y))]
    public float Y
    {
        get => m_Source.y;
        set => m_Source.y = value;
    }

    [LuaMember(nameof(Vector3.z))]
    public float Z
    {
        get => m_Source.z;
        set => m_Source.z = value;
    }

    [LuaMember(nameof(Vector3.normalized))]
    public LuaVector3 Normalized => new(m_Source.normalized);

    [LuaMember(nameof(Vector3.magnitude))]
    public float Magnitude => m_Source.magnitude;

    [LuaMember(nameof(Vector3.sqrMagnitude))]
    public float SqrMagnitude => m_Source.sqrMagnitude;

    [LuaMember(LuaNamingStandard.CONSTRUCTOR_FUNCTION_NAME)]
    public static LuaVector3 New(float x, float y, float z) => new(x, y, z);

    [LuaMember(nameof(Vector3.Set))]
    public void Set(float newX, float newY, float newZ) => m_Source.Set(newX, newY, newZ);

    [LuaIgnoreMember]
    public void Set(Vector3 vector3) => m_Source = vector3;

    [LuaIgnoreMember]
    public void Set(LuaVector3 vector3) => Set(vector3.ToUnityVector3());

    [LuaMember(nameof(ScaleInPlace))]
    public void ScaleInPlace(LuaVector3 vector3)
    {
        X *= vector3.X;
        Y *= vector3.Y;
        Z *= vector3.Z;
    }

    [LuaMember(nameof(Vector3.Normalize))]
    public void Normalize() => m_Source.Normalize();

    [LuaIgnoreMember]
    public bool Equals(LuaVector3 other) => X == other.X && Y == other.Y && Z == other.Z;

    [LuaIgnoreMember]
    public bool Equals(Vector3 other) => X == other.x && Y == other.y && Z == other.z;

    [LuaMember(nameof(Copy))]
    public LuaVector3 Copy() => new(this);

    [LuaIgnoreMember]
    public override bool Equals(object obj) => obj switch
    {
        Vector3 vector3 => Equals(vector3),
        LuaVector3 vector3 => Equals(vector3),
        _ => false,
    };

    [LuaIgnoreMember]
    public override int GetHashCode() => m_Source.GetHashCode();

    [LuaIgnoreMember]
    public override string ToString() => m_Source.ToString();

    [LuaMember(nameof(AddInPlace))]
    public void AddInPlace(LuaVector3 b) => Set(ToUnityVector3() + b.ToUnityVector3());

    [LuaMember(nameof(SubtractInPlace))]
    public void SubtractInPlace(LuaVector3 b) => Set(ToUnityVector3() - b.ToUnityVector3());

    [LuaMember(nameof(NegateInPlace))]
    public void NegateInPlace() => Set(-ToUnityVector3());

    [LuaMember(nameof(CrossInPlace))]
    public void CrossInPlace(LuaVector3 rhs) => Set(Vector3.Cross(ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(ReflectInPlace))]
    public void ReflectInPlace(LuaVector3 inNormal) => Set(Vector3.Reflect(ToUnityVector3(), inNormal.ToUnityVector3()));

    [LuaMember(nameof(ProjectInPlace))]
    public void ProjectInPlace(LuaVector3 onNormal) => Set(Vector3.Project(ToUnityVector3(), onNormal.ToUnityVector3()));

    [LuaMember(nameof(ProjectOnPlaneInPlace))]
    public void ProjectOnPlaneInPlace(LuaVector3 planeNormal) => Set(Vector3.ProjectOnPlane(ToUnityVector3(), planeNormal.ToUnityVector3()));

    [LuaMember(nameof(ClampMagnitudeInPlace))]
    public void ClampMagnitudeInPlace(float maxLength) => Set(Vector3.ClampMagnitude(ToUnityVector3(), maxLength));

    [LuaMember(nameof(MinInPlace))]
    public void MinInPlace(LuaVector3 rhs) => Set(Vector3.Min(ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(MaxInPlace))]
    public void MaxInPlace(LuaVector3 rhs) => Set(Vector3.Max(ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(SmoothDampInPlace))]
    public void SmoothDampInPlace(LuaVector3 target, LuaVector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        var currentVelocityUnityVector3 = currentVelocity.ToUnityVector3();
        Set(Vector3.SmoothDamp(ToUnityVector3(), target.ToUnityVector3(), ref currentVelocityUnityVector3, smoothTime, maxSpeed, deltaTime));
        currentVelocity.Set(currentVelocityUnityVector3);
    }

    [LuaMember(nameof(LerpInPlace))]
    public void LerpInPlace(LuaVector3 b, float t) => Set(Vector3.Lerp(ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(LerpUnclampedInPlace))]
    public void LerpUnclampedInPlace(LuaVector3 b, float t) => Set(Vector3.LerpUnclamped(ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(SlerpInPlace))]
    public void SlerpInPlace(LuaVector3 b, float t) => Set(Vector3.Slerp(ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(SlerpUnclampedInPlace))]
    public void SlerpUnclampedInPlace(LuaVector3 b, float t) => Set(Vector3.SlerpUnclamped(ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(MoveTowardsInPlace))]
    public void MoveTowardsInPlace(LuaVector3 target, float maxDistanceDelta) => Set(Vector3.MoveTowards(ToUnityVector3(), target.ToUnityVector3(), maxDistanceDelta));

    [LuaMember(nameof(RotateTowardsInPlace))]
    public void RotateTowardsInPlace(LuaVector3 target, float maxRadiansDelta, float maxMagnitudeDelta) => Set(Vector3.RotateTowards(ToUnityVector3(), target.ToUnityVector3(), maxRadiansDelta, maxMagnitudeDelta));

    [LuaMetamethod(LuaObjectMetamethod.Add)]
    public static LuaVector3 Add(LuaVector3 a, LuaVector3 b) => new(a.ToUnityVector3() + b.ToUnityVector3());

    [LuaMetamethod(LuaObjectMetamethod.Sub)]
    public static LuaVector3 Subtract(LuaVector3 a, LuaVector3 b) => new(a.ToUnityVector3() - b.ToUnityVector3());

    [LuaMetamethod(LuaObjectMetamethod.Unm)]
    public static LuaVector3 Negate(LuaVector3 a) => new(-a.ToUnityVector3());

    [LuaMetamethod(LuaObjectMetamethod.Eq)]
    public static bool Equals(LuaVector3 lhs, LuaVector3 rhs) => lhs.ToUnityVector3() == rhs.ToUnityVector3();

    [LuaMember(nameof(Vector3.Cross))]
    public static LuaVector3 Cross(LuaVector3 lhs, LuaVector3 rhs) => new(Vector3.Cross(lhs.ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(Vector3.Reflect))]
    public static LuaVector3 Reflect(LuaVector3 inDirection, LuaVector3 inNormal) => new(Vector3.Reflect(inDirection.ToUnityVector3(), inNormal.ToUnityVector3()));

    [LuaMember(nameof(Vector3.Project))]
    public static LuaVector3 Project(LuaVector3 vector, LuaVector3 onNormal) => new(Vector3.Project(vector.ToUnityVector3(), onNormal.ToUnityVector3()));

    [LuaMember(nameof(Vector3.ProjectOnPlane))]
    public static LuaVector3 ProjectOnPlane(LuaVector3 vector, LuaVector3 planeNormal) => new(Vector3.ProjectOnPlane(vector.ToUnityVector3(), planeNormal.ToUnityVector3()));

    [LuaMember(nameof(Vector3.Dot))]
    public static float Dot(LuaVector3 lhs, LuaVector3 rhs) => Vector3.Dot(lhs.ToUnityVector3(), rhs.ToUnityVector3());

    [LuaMember(nameof(Vector3.Angle))]
    public static float Angle(LuaVector3 from, LuaVector3 to) => Vector3.Angle(from.ToUnityVector3(), to.ToUnityVector3());

    [LuaMember(nameof(Vector3.SignedAngle))]
    public static float SignedAngle(LuaVector3 from, LuaVector3 to, LuaVector3 axis) => Vector3.SignedAngle(from.ToUnityVector3(), to.ToUnityVector3(), axis.ToUnityVector3());

    [LuaMember(nameof(Vector3.Distance))]
    public static float Distance(LuaVector3 a, LuaVector3 b) => Vector3.Distance(a.ToUnityVector3(), b.ToUnityVector3());

    [LuaMember(nameof(Vector3.ClampMagnitude))]
    public static LuaVector3 ClampMagnitude(LuaVector3 vector, float maxLength) => new(Vector3.ClampMagnitude(vector.ToUnityVector3(), maxLength));

    [LuaMember(nameof(Vector3.Min))]
    public static LuaVector3 Min(LuaVector3 lhs, LuaVector3 rhs) => new(Vector3.Min(lhs.ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(Vector3.Max))]
    public static LuaVector3 Max(LuaVector3 lhs, LuaVector3 rhs) => new(Vector3.Max(lhs.ToUnityVector3(), rhs.ToUnityVector3()));

    [LuaMember(nameof(Vector3.SmoothDamp))]
    public static LuaVector3 SmoothDamp(LuaVector3 current, LuaVector3 target, LuaVector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        var currentVelocityUnityVector3 = currentVelocity.ToUnityVector3();
        LuaVector3 result = new(Vector3.SmoothDamp(current.ToUnityVector3(), target.ToUnityVector3(), ref currentVelocityUnityVector3, smoothTime, maxSpeed, deltaTime));
        currentVelocity.Set(currentVelocityUnityVector3);
        return result;
    }

    [LuaMember(nameof(Vector3.Lerp))]
    public static LuaVector3 Lerp(LuaVector3 a, LuaVector3 b, float t) => new(Vector3.Lerp(a.ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(Vector3.LerpUnclamped))]
    public static LuaVector3 LerpUnclamped(LuaVector3 a, LuaVector3 b, float t) => new(Vector3.LerpUnclamped(a.ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(Vector3.Slerp))]
    public static LuaVector3 Slerp(LuaVector3 a, LuaVector3 b, float t) => new(Vector3.Slerp(a.ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(Vector3.SlerpUnclamped))]
    public static LuaVector3 SlerpUnclamped(LuaVector3 a, LuaVector3 b, float t) => new(Vector3.SlerpUnclamped(a.ToUnityVector3(), b.ToUnityVector3(), t));

    [LuaMember(nameof(Vector3.MoveTowards))]
    public static LuaVector3 MoveTowards(LuaVector3 current, LuaVector3 target, float maxDistanceDelta) => new(Vector3.MoveTowards(current.ToUnityVector3(), target.ToUnityVector3(), maxDistanceDelta));

    [LuaMember(nameof(Vector3.RotateTowards))]
    public static LuaVector3 RotateTowards(LuaVector3 current, LuaVector3 target, float maxRadiansDelta, float maxMagnitudeDelta) => new(Vector3.RotateTowards(current.ToUnityVector3(), target.ToUnityVector3(), maxRadiansDelta, maxMagnitudeDelta));

    [LuaMember(nameof(Vector3.Scale))]
    public static LuaVector3 Scale(LuaVector3 a, LuaVector3 b) => new(Vector3.Scale(a.ToUnityVector3(), b.ToUnityVector3()));

    [LuaMember(nameof(Vector3.OrthoNormalize))]
    public static void OrthoNormalize3(LuaVector3 normal, LuaVector3 tangent, LuaVector3 binormal)
    {
        var normalVector = normal.ToUnityVector3();
        var tangentVector = tangent.ToUnityVector3();
        var binormalVector = binormal.ToUnityVector3();
        Vector3.OrthoNormalize(ref normalVector, ref tangentVector, ref binormalVector);
        normal.Set(normalVector);
        tangent.Set(tangentVector);
        binormal.Set(binormalVector);
    }

    [LuaMember(nameof(Vector3.AngleBetween))]
    public static float AngleBetween(LuaVector3 from, LuaVector3 to) => Vector3.Angle(from.ToUnityVector3(), to.ToUnityVector3());

    [LuaMember(nameof(ToVector2))]
    public LuaVector3 ToVector2() => new(this);

    [LuaIgnoreMember]
    public Vector3 ToUnityVector3() => new(X, Y, Z);

    ILuaUserData ILuaCopiable.Copy() => Copy();
}