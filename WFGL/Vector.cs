namespace WFGL;
public interface IVector<T> where T : struct, IVector<T>
{
    public float Magnitude();
    public T Normalize();
}
public struct Vector2(float x, float y) : IVector<Vector2>
{
    public readonly static Vector2 Zero = new(0, 0);
    public readonly static Vector2 One = new(1,1);

    public readonly static Vector2 Left = new(-1,0);
    public readonly static Vector2 Right = new(1,0);
    public readonly static Vector2 Up = new(0,-1);
    public readonly static Vector2 Down = new(0,1);

    public float X = x;
    public float Y = y;

    public float Magnitude() => (float)Math.Sqrt(X * X + Y * Y);
    public Vector2 Normalize()
    {
        float magnitude = Magnitude();
        if (magnitude > 0)
        {
            X /= magnitude;
            Y /= magnitude;
        }
        return this;
    }

    public static implicit operator Vector2(float a) => new(a, a);
    public static explicit operator Vector2(Vector3 a) => new(a.X,a.Y);
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vector2 operator %(Vector2 a, Vector2 b) => new(a.X % b.X, a.Y % b.Y);


    public static Vector2 operator +(Vector2 a, float b) => new(a.X + b, a.Y + b);
    public static Vector2 operator -(Vector2 a, float b) => new(a.X - b, a.Y - b);
    public static Vector2 operator *(Vector2 a, float b) => new(a.X * b, a.Y * b);
    public static Vector2 operator /(Vector2 a, float b) => new(a.X / b, a.Y / b);
    public static Vector2 operator %(Vector2 a, float b) => new(a.X % b, a.Y % b);

    public readonly override string ToString() => $"V2({X};{Y})";
}


public struct Vector3(float x,float y, float z) : IVector<Vector3>
{
    public readonly static Vector2 Zero = new(0, 0);
    public readonly static Vector2 One = new(1, 1);


    public float X = x;
    public float Y = y;
    public float Z = z;

    public float Magnitude() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public Vector3 Normalize()
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


    public static implicit operator Vector3(float a) => new(a, a, a);
    public static explicit operator Vector3(Vector2 a) => new(a.X, a.Y, 0);

    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vector3 operator /(Vector3 a, Vector3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Vector3 operator %(Vector3 a, Vector3 b) => new(a.X % b.X, a.Y % b.Y, a.Z % b.Z);


    public static Vector3 operator +(Vector3 a, float b) => new(a.X + b, a.Y + b, a.Z + b);
    public static Vector3 operator -(Vector3 a, float b) => new(a.X - b, a.Y - b, a.Z - b);
    public static Vector3 operator *(Vector3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vector3 operator /(Vector3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);
    public static Vector3 operator %(Vector3 a, float b) => new(a.X % b, a.Y % b, a.Z % b);

    public readonly override string ToString() => $"V3({X};{Y},{Z})";
}