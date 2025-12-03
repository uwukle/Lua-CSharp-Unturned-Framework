using Lua.Scripting;
using Lua.Scripting.Abstraction;
using System;
using UnityEngine;

namespace Lua.Unity;

[LuaObject(nameof(Vector2))]
public sealed partial class LuaVector2(float x, float y) : ILuaNormalizable, ILuaCopiable<LuaVector2>, IEquatable<LuaVector2>, IEquatable<Vector2>
{
    [LuaIgnoreMember]
    private Vector2 m_Source = new(x, y);

    public LuaVector2(Vector2 vector2) : this(vector2.x, vector2.y) { }

    public LuaVector2(Vector3 vector3) : this(vector3.x, vector3.y) { }

    public LuaVector2(Vector4 vector4) : this(vector4.x, vector4.y) { }

    public LuaVector2(Rect vector4) : this(vector4.x, vector4.y) { }

    public LuaVector2(LuaVector2 vector2) : this(vector2.X, vector2.Y) { }

    public LuaVector2(LuaVector3 vector3) : this(vector3.X, vector3.Y) { }

    public LuaVector2() : this(0.0f, 0.0f) { }

    [LuaMember(nameof(Vector2.zero))]
    public static LuaVector2 Zero => new(Vector2.zero);

    [LuaMember(nameof(Vector2.one))]
    public static LuaVector2 One => new(Vector2.one);

    [LuaMember(nameof(Vector2.up))]
    public static LuaVector2 Up => new(Vector2.up);

    [LuaMember(nameof(Vector2.down))]
    public static LuaVector2 Down => new(Vector2.down);

    [LuaMember(nameof(Vector2.left))]
    public static LuaVector2 Left => new(Vector2.left);

    [LuaMember(nameof(Vector2.right))]
    public static LuaVector2 Right => new(Vector2.right);

    [LuaMember(nameof(Vector2.positiveInfinity))]
    public static LuaVector2 PositiveInfinity => new(Vector2.positiveInfinity);

    [LuaMember(nameof(Vector2.negativeInfinity))]
    public static LuaVector2 NegativeInfinity => new(Vector2.negativeInfinity);

    [LuaMember(nameof(Vector2.x))]
    public float X
    {
        get => m_Source.x;
        set => m_Source.x = value;
    }

    [LuaMember(nameof(Vector2.y))]
    public float Y
    {
        get => m_Source.y;
        set => m_Source.y = value;
    }

    [LuaMember(nameof(Vector2.normalized))]
    public LuaVector2 Normalized => new(m_Source.normalized);

    [LuaMember(nameof(Vector2.magnitude))]
    public float Magnitude => m_Source.magnitude;

    [LuaMember(nameof(Vector2.sqrMagnitude))]
    public float SqrMagnitude => m_Source.sqrMagnitude;

    [LuaMember(LuaNamingStandard.CONSTRUCTOR_FUNCTION_NAME)]
    public static LuaVector2 New(float x, float y) => new(x, y);

    [LuaMember(nameof(Vector2.Set))]
    public void Set(float x, float y) => m_Source.Set(x, y);

    [LuaIgnoreMember]
    public void Set(Vector2 vector2) => m_Source = vector2;

    [LuaIgnoreMember]
    public void Set(LuaVector2 vector2) => Set(vector2.ToUnityVector2());

    [LuaMember(nameof(ScaleInPlace))]
    public void ScaleInPlace(LuaVector2 vector2)
    {
        X *= vector2.X;
        Y *= vector2.Y;
    }

    [LuaMember(nameof(Vector2.Normalize))]
    public void Normalize() => m_Source.Normalize();

    [LuaIgnoreMember]
    public bool Equals(LuaVector2 other) => X == other.X && Y == other.Y;

    [LuaIgnoreMember]
    public bool Equals(Vector2 other) => X == other.x && Y == other.y;

    [LuaMember(nameof(Copy))]
    public LuaVector2 Copy() => new(this);

    [LuaIgnoreMember]
    public override bool Equals(object obj) => obj switch
    {
        Vector2 vector2 => Equals(vector2),
        LuaVector2 vector2 => Equals(vector2),
        _ => false,
    };

    [LuaIgnoreMember]
    public override int GetHashCode() => m_Source.GetHashCode();

    [LuaIgnoreMember]
    public override string ToString() => m_Source.ToString();

    [LuaMember(nameof(AddInPlace))]
    public void AddInPlace(LuaVector2 b) => Set(ToUnityVector2() + b.ToUnityVector2());

    [LuaMember(nameof(SubtractInPlace))]
    public void SubtractInPlace(LuaVector2 b) => Set(ToUnityVector2() - b.ToUnityVector2());

    [LuaMember(nameof(MultiplyInPlace))]
    public void MultiplyInPlace(LuaVector2 b) => Set(ToUnityVector2() * b.ToUnityVector2());

    [LuaMember(nameof(DivideInPlace))]
    public void DivideInPlace(LuaVector2 b) => Set(ToUnityVector2() / b.ToUnityVector2());

    [LuaMember(nameof(NegateInPlace))]
    public void NegateInPlace() => Set(-ToUnityVector2());

    [LuaMember(nameof(ReflectInPlace))]
    public void ReflectInPlace(LuaVector2 inNormal) => Set(Vector2.Reflect(ToUnityVector2(), inNormal.ToUnityVector2()));

    [LuaMember(nameof(PerpendicularInPlace))]
    public void PerpendicularInPlace() => Set(Vector2.Perpendicular(ToUnityVector2()));

    [LuaMember(nameof(ClampMagnitudeInPlace))]
    public void ClampMagnitudeInPlace(float maxLength) => Set(Vector2.ClampMagnitude(ToUnityVector2(), maxLength));

    [LuaMember(nameof(MinInPlace))]
    public void MinInPlace(LuaVector2 rhs) => Set(Vector2.Min(ToUnityVector2(), rhs.ToUnityVector2()));

    [LuaMember(nameof(MaxInPlace))]
    public void MaxInPlace(LuaVector2 rhs) => Set(Vector2.Max(ToUnityVector2(), rhs.ToUnityVector2()));

    [LuaMember(nameof(SmoothDampInPlace))]
    public void SmoothDampInPlace(LuaVector2 target, LuaVector2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        var currentVelocityUnityVector2 = currentVelocity.ToUnityVector2();
        Set(Vector2.SmoothDamp(ToUnityVector2(), target.ToUnityVector2(), ref currentVelocityUnityVector2, smoothTime, maxSpeed, deltaTime));
        currentVelocity.Set(currentVelocityUnityVector2);
    }

    [LuaMember(nameof(LerpInPlace))]
    public void LerpInPlace(LuaVector2 b, float t) => Set(Vector2.Lerp(ToUnityVector2(), b.ToUnityVector2(), t));

    [LuaMember(nameof(LerpUnclampedInPlace))]
    public void LerpUnclampedInPlace(LuaVector2 b, float t) => Set(Vector2.LerpUnclamped(ToUnityVector2(), b.ToUnityVector2(), t));

    [LuaMember(nameof(MoveTowardsInPlace))]
    public void MoveTowardsInPlace(LuaVector2 target, float maxDistanceDelta) => Set(Vector2.MoveTowards(ToUnityVector2(), target.ToUnityVector2(), maxDistanceDelta));

    [LuaMetamethod(LuaObjectMetamethod.Add)]
    public static LuaVector2 Add(LuaVector2 a, LuaVector2 b) => new(a.ToUnityVector2() + b.ToUnityVector2());

    [LuaMetamethod(LuaObjectMetamethod.Sub)]
    public static LuaVector2 Subtract(LuaVector2 a, LuaVector2 b) => new(a.ToUnityVector2() - b.ToUnityVector2());

    [LuaMetamethod(LuaObjectMetamethod.Mul)]
    public static LuaVector2 Multiply(LuaVector2 a, LuaVector2 b) => new(a.ToUnityVector2() * b.ToUnityVector2());

    [LuaMetamethod(LuaObjectMetamethod.Div)]
    public static LuaVector2 Divide(LuaVector2 a, LuaVector2 b) => new(a.ToUnityVector2() / b.ToUnityVector2());

    [LuaMetamethod(LuaObjectMetamethod.Unm)]
    public static LuaVector2 Negate(LuaVector2 a) => new(-a.ToUnityVector2());

    [LuaMetamethod(LuaObjectMetamethod.Eq)]
    public static bool Equals(LuaVector2 lhs, LuaVector2 rhs) => lhs.ToUnityVector2() == rhs.ToUnityVector2();

    [LuaMember(nameof(Vector2.Reflect))]
    public static LuaVector2 Reflect(LuaVector2 inDirection, LuaVector2 inNormal) => new(Vector2.Reflect(inDirection.ToUnityVector2(), inNormal.ToUnityVector2()));

    [LuaMember(nameof(Vector2.Perpendicular))]
    public static LuaVector2 Perpendicular(LuaVector2 inDirection) => new(Vector2.Perpendicular(inDirection.ToUnityVector2()));

    [LuaMember(nameof(Vector2.Dot))]
    public static float Dot(LuaVector2 lhs, LuaVector2 rhs) => Vector2.Dot(lhs.ToUnityVector2(), rhs.ToUnityVector2());

    [LuaMember(nameof(Vector2.Angle))]
    public static float Angle(LuaVector2 from, LuaVector2 to) => Vector2.Angle(from.ToUnityVector2(), to.ToUnityVector2());

    [LuaMember(nameof(Vector2.SignedAngle))]
    public static float SignedAngle(LuaVector2 from, LuaVector2 to) => Vector2.SignedAngle(from.ToUnityVector2(), to.ToUnityVector2());

    [LuaMember(nameof(Vector2.Distance))]
    public static float Distance(LuaVector2 a, LuaVector2 b) => Vector2.Distance(a.ToUnityVector2(), b.ToUnityVector2());

    [LuaMember(nameof(Vector2.ClampMagnitude))]
    public static LuaVector2 ClampMagnitude(LuaVector2 vector, float maxLength) => new(Vector2.ClampMagnitude(vector.ToUnityVector2(), maxLength));

    [LuaMember(nameof(Vector2.Min))]
    public static LuaVector2 Min(LuaVector2 lhs, LuaVector2 rhs) => new(Vector2.Min(lhs.ToUnityVector2(), rhs.ToUnityVector2()));

    [LuaMember(nameof(Vector2.Max))]
    public static LuaVector2 Max(LuaVector2 lhs, LuaVector2 rhs) => new(Vector2.Max(lhs.ToUnityVector2(), rhs.ToUnityVector2()));

    [LuaMember(nameof(Vector2.SmoothDamp))]
    public static LuaVector2 SmoothDamp(LuaVector2 current, LuaVector2 target, LuaVector2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        var currentVelocityUnityVector2 = currentVelocity.ToUnityVector2();
        LuaVector2 result = new(Vector2.SmoothDamp(current.ToUnityVector2(), target.ToUnityVector2(), ref currentVelocityUnityVector2, smoothTime, maxSpeed, deltaTime));
        currentVelocity.Set(currentVelocityUnityVector2);
        return result;
    }

    [LuaMember(nameof(Vector2.Lerp))]
    public static LuaVector2 Lerp(LuaVector2 a, LuaVector2 b, float t) => new(Vector2.Lerp(a.ToUnityVector2(), b.ToUnityVector2(), t));

    [LuaMember(nameof(Vector2.LerpUnclamped))]
    public static LuaVector2 LerpUnclamped(LuaVector2 a, LuaVector2 b, float t) => new(Vector2.LerpUnclamped(a.ToUnityVector2(), b.ToUnityVector2(), t));

    [LuaMember(nameof(Vector2.MoveTowards))]
    public static LuaVector2 MoveTowards(LuaVector2 current, LuaVector2 target, float maxDistanceDelta) => new(Vector2.MoveTowards(current.ToUnityVector2(), target.ToUnityVector2(), maxDistanceDelta));

    [LuaMember(nameof(ToVector3))]
    public LuaVector3 ToVector3() => new(this);

    [LuaIgnoreMember]
    public Vector2 ToUnityVector2() => new(X, Y);

    ILuaUserData ILuaCopiable.Copy() => Copy();
}