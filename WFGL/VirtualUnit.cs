namespace WFGL;

/// <summary>
/// Amount of pixels per vector. Defines refrence for drawing objects in appropriate scale.
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
public struct VirtualUnit(float x, float y)
{
    public const int SCALING = 5; // one vec = (1/5 ,1/5) of screen size 
    public float FactorX { get; set; } = x;
    public float FactorY { get; set; } = y;

    /// <summary>
    /// Takes the min value of x and y. 
    /// </summary>
    public VirtualUnit Normalize()
    {
        float average = Math.Min(FactorX, FactorY);
        FactorX = average;
        FactorY = average;
        return this;
    }

    /// <summary>
    /// Takes the avrage of x and y. 
    /// </summary>
    public VirtualUnit NormalizeByAvarage()
    {
        float average = (FactorX + FactorY) / 2f;
        FactorX = average;
        FactorY = average;
        return this;
    }

    public static float VirtualizeToFactor(int pixels) => pixels / SCALING; // get number of virtual units on the screen
    public static float DevirtualizeFactor(int units) => units * SCALING; // reverse operation above

    public readonly Size GetSize() =>
        new((int)FactorX * SCALING, (int)FactorX * SCALING);


    public static implicit operator VirtualUnit(float a) => new(a, a);
    public static VirtualUnit operator +(VirtualUnit a, VirtualUnit b) => new(a.FactorX + b.FactorX, a.FactorY + b.FactorY);
    public static VirtualUnit operator -(VirtualUnit a, VirtualUnit b) => new(a.FactorX - b.FactorX, a.FactorY - b.FactorY);
    public static VirtualUnit operator *(VirtualUnit a, VirtualUnit b) => new(a.FactorX * b.FactorX, a.FactorY * b.FactorY);
    public static VirtualUnit operator /(VirtualUnit a, VirtualUnit b) => new(a.FactorX / b.FactorX, a.FactorY / b.FactorY);
    public static VirtualUnit operator %(VirtualUnit a, VirtualUnit b) => new(a.FactorX % b.FactorX, a.FactorY % b.FactorY);

    public readonly override string ToString() => $"VU({FactorX};{FactorY})";
}
