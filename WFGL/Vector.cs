namespace WFGL;

public struct Vector2(float x, float y)
{
    public readonly static Vector2 Zero = new(0,0);
    public readonly static Vector2 One = new(1,1);
    public readonly static Vector2 Half = new(0.5f,0.5f);

    public readonly static Vector2 Left = new(-1,0);
    public readonly static Vector2 Right = new(1,0);
    public readonly static Vector2 Up = new(0,-1);
    public readonly static Vector2 Down = new(0,1);

    public float X = x;
    public float Y = y;

    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }
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

    public Vector2 Cut(Vector2 size, Vector2 bounds)
    {
        X = Math.Max(0, Math.Min(bounds.X - size.X, X));
        Y = Math.Max(0, Math.Min(bounds.Y - size.Y, Y));
        return this;
    }
    public static implicit operator Vector2(float a) => new(a, a);
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

    public readonly override string ToString() => $"({X};{Y})";
}