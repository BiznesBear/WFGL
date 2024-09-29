namespace WFGL;

public struct VirtualUnit(float x, float y)
{
    public const int SCALING = 5;
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
    
    public static float VirtualizeToFactor(int pixels) => pixels / SCALING;
    public static float DevirtualizeFactor(int units) => units * SCALING;


    public static implicit operator VirtualUnit(float a) => new(a, a);
    public static VirtualUnit operator +(VirtualUnit a, VirtualUnit b) => new(a.FactorX + b.FactorX, a.FactorY + b.FactorY);
    public static VirtualUnit operator -(VirtualUnit a, VirtualUnit b) => new(a.FactorX - b.FactorX, a.FactorY - b.FactorY);
    public static VirtualUnit operator *(VirtualUnit a, VirtualUnit b) => new(a.FactorX * b.FactorX, a.FactorY * b.FactorY);
    public static VirtualUnit operator /(VirtualUnit a, VirtualUnit b) => new(a.FactorX / b.FactorX, a.FactorY / b.FactorY);
    public static VirtualUnit operator %(VirtualUnit a, VirtualUnit b) => new(a.FactorX % b.FactorX, a.FactorY % b.FactorY);
}
public static class Converts
{
    #region Push

    public static Point PushToPoint(this Size size) => new(size.Width, size.Height);
    public static Size PushToSize(this Point pixel) => new(pixel.X, pixel.Y);

    #endregion

    #region Converts
    public static Vector2 ToVector2(this Point point, VirtualUnit vunit) => new(point.X / vunit.FactorX, point.Y / vunit.FactorY);
    public static Vector2 ToVector2(this Size size, VirtualUnit vunit) => size.PushToPoint().ToVector2(vunit);
    public static Point ToPoint(this Vector2 vector2, VirtualUnit vunit) => new((int)(vector2.X * vunit.FactorX), (int)(vector2.Y * vunit.FactorY));
    public static Size ToSize(this Vector2 vector2, VirtualUnit vunit) => vector2.ToPoint(vunit).PushToSize();
    #endregion

    /// <summary>
    /// Used for drawing real scale of images.
    /// </summary>
    public static Point VirtualizePixel(this Point pixel, Camera camera) => new((int)(pixel.X * camera.Scaler.FactorX), (int)(pixel.Y * camera.Scaler.FactorY));

    /// <summary>
    /// Used for drawing real scale of images.
    /// </summary>
    public static Size VirtualizePixel(this Size pixel, Camera camera) => pixel.PushToPoint().VirtualizePixel(camera).PushToSize();
}