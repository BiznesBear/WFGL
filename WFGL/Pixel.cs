namespace WFGL;

public struct Pixel(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public static readonly Pixel Zero = new(0, 0);

    public void Offset(int x, int y)
    {
        unchecked
        {
            X += x;
            Y += y;
        }
    }

    public static Pixel operator +(Pixel a, Pixel b) => new(a.X + b.X, a.Y + b.Y);
    public static Pixel operator -(Pixel a, Pixel b) => new(a.X - b.X, a.Y - b.Y);
    public static Pixel operator *(Pixel a, Pixel b) => new(a.X * b.X, a.Y * b.Y);
    public static Pixel operator /(Pixel a, Pixel b) => new(a.X / b.X, a.Y / b.Y);
    public readonly override string ToString() => $"({X},{Y})";
}
