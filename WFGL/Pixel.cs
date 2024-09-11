namespace WFGL;

public struct Pixel(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public static readonly Pixel Zero = new(0,0);

    public readonly override string ToString() => $"({X},{Y})";
}
