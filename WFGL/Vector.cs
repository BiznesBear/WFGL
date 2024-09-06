using WFGL.Core;

namespace WFGL;

public struct Vector2(float x, float y)
{
    public readonly static Vector2 Zero = new(0,0);
    public readonly static Vector2 One = new(1,1);

    public readonly static Vector2 Left = new(-1,0);
    public readonly static Vector2 Right = new(1,0);
    public readonly static Vector2 Up = new(0,1);
    public readonly static Vector2 Down = new(0,-1);

    public float X = x;
    public float Y = y;


    public Vector2 Fix()
    {
        return One;
    }

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

    public Vector2 CutToBounds(Sprite sprite, GameWindow window)
    {
        X = Math.Max(0, Math.Min(window.ClientSize.Width - sprite.RealSize.X, X));
        Y = Math.Max(0, Math.Min(window.ClientSize.Height - sprite.RealSize.Y, Y));
        return this;
    }

    public Size ToSize() => new((int)X, (int)Y);
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vector2 operator %(Vector2 a, Vector2 b) => new(a.X % b.X, a.Y % b.Y);
    public override string ToString() => $"({X},{Y})";
}
