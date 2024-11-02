using System.Numerics;
namespace WFGL.Physics;
public interface IVec<T> where T : struct, IVec<T>
{
    public float Magnitude();
    public T Normalize();
}
public struct Vec2(float x, float y) : IVec<Vec2>
{
    public readonly static Vec2 Zero = new(0, 0);
    public readonly static Vec2 One = new(1, 1);

    public readonly static Vec2 Left = new(-1, 0);
    public readonly static Vec2 Right = new(1, 0);
    public readonly static Vec2 Up = new(0, -1);
    public readonly static Vec2 Down = new(0, 1);

    public float X = x;
    public float Y = y;

    public float Magnitude() => (float)Math.Sqrt(X * X + Y * Y);
    public Vec2 Normalize()
    {
        float magnitude = Magnitude();
        if (magnitude > 0)
        {
            X /= magnitude;
            Y /= magnitude;
        }
        return this;
    }

    public static implicit operator Vec2(float a) => new(a, a);
    public static explicit operator Vec2(Vec3 a) => new(a.X, a.Y);
    public static explicit operator Vec2(Vector2 a) => new(a.X, a.Y);
    public static explicit operator Vec2(Vector3 a) => new(a.X, a.Y);
    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator *(Vec2 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator /(Vec2 a, Vec2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator %(Vec2 a, Vec2 b) => new(a.X % b.X, a.Y % b.Y);


    public static Vec2 operator +(Vec2 a, float b) => new(a.X + b, a.Y + b);
    public static Vec2 operator -(Vec2 a, float b) => new(a.X - b, a.Y - b);
    public static Vec2 operator *(Vec2 a, float b) => new(a.X * b, a.Y * b);
    public static Vec2 operator /(Vec2 a, float b) => new(a.X / b, a.Y / b);
    public static Vec2 operator %(Vec2 a, float b) => new(a.X % b, a.Y % b);

    public readonly override string ToString() => $"Vec2({X};{Y})";
}


public struct Vec3(float x, float y, float z) : IVec<Vec3>
{
    public readonly static Vec2 Zero = new(0, 0);
    public readonly static Vec2 One = new(1, 1);


    public float X = x;
    public float Y = y;
    public float Z = z;

    public float Magnitude() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public Vec3 Normalize()
    {
        float magnitude = Magnitude();
        if (magnitude > 0)
        {
            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
        }
        return this;
    }

    public Vec2 Project(Vec2 screenCenter, float distance = 5, float scale = 400)
    {
        float factor = distance / (distance + Z);
        float x2d = X * factor * scale + screenCenter.X;
        float y2d = Y * factor * scale + screenCenter.Y;

        return new Vec2(x2d, y2d);
    }

    public static implicit operator Vec3(float a) => new(a, a, a);
    public static explicit operator Vec3(Vec2 a) => new(a.X, a.Y, 0);
    public static explicit operator Vec3(Vector3 a) => new(a.X, a.Y, a.Z);

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator /(Vec3 a, Vec3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Vec3 operator %(Vec3 a, Vec3 b) => new(a.X % b.X, a.Y % b.Y, a.Z % b.Z);


    public static Vec3 operator +(Vec3 a, float b) => new(a.X + b, a.Y + b, a.Z + b);
    public static Vec3 operator -(Vec3 a, float b) => new(a.X - b, a.Y - b, a.Z - b);
    public static Vec3 operator *(Vec3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);
    public static Vec3 operator %(Vec3 a, float b) => new(a.X % b, a.Y % b, a.Z % b);

    public readonly override string ToString() => $"Vec3({X};{Y},{Z})";
}