namespace WFGL;

public struct VirtualUnit(float x, float y)
{
    public const int SCALING = 5;
    public float FactorX { get; set; } = x;
    public float FactorY { get; set; } = y;

    /// <summary>
    /// Takes the average of x and y. 
    /// </summary>
    public VirtualUnit Normalize()
    {
        float average = Math.Min(FactorX, FactorY);
        FactorX = average;
        FactorY = average;
        return this;
    }

    public VirtualUnit NormalizeByAvarage()
    {
        float average = (FactorX + FactorY) / 2f;
        FactorX = average;
        FactorY = average;
        return this;
    }
    
    public static float VirtualizeToFactor(int pixels) => pixels / SCALING;
    public static float DevirtualizeFactor(int units) => units * SCALING;
}
public static class Converts
{
    // TODO: Add automatic virtualization (converting game logic unit to pixels and vice versa)
    // NOTE: It funcs below doesn't have automatic virtualization. That means (1,1) in vectors equals (1,1) in pixels. This is force pushed to be somthing
    // TODO: Add choosing (with camera) the optimal scale (one for all axis) or mean of both
    public static Point PushToPoint(this Vector2 vector2) => new((int)vector2.X, (int)vector2.Y);
    public static Size PushToSize(this Vector2 vector2) => new((int)vector2.X, (int)vector2.Y);
    public static Pixel PushToPixel(this Vector2 vector2) => new((int)vector2.X, (int)vector2.Y);
    public static Pixel PushToPixel(this Point vector2) => new((int)vector2.X, (int)vector2.Y);

    public static Point PushToPoint(this Pixel vector2) => new((int)vector2.X, (int)vector2.Y);
    public static Size PushToSize(this Pixel vector2) => new((int)vector2.X, (int)vector2.Y);

    // this doesn't have virtualization too
    public static Vector2 PushToVector2(this Size size) => new(size.Width, size.Height);
    public static Vector2 PushToVector2(this Point point) => new(point.X, point.Y);
    public static Vector2 PushToVector2(this Pixel pixel) => new(pixel.X, pixel.Y);


    // real converts 
    public static Vector2 ToVector2(this Pixel pixel, VirtualUnit vunit) => new((pixel.X / vunit.FactorX), (pixel.Y / vunit.FactorY));
    public static Pixel ToPixel(this Vector2 vector2, VirtualUnit vunit) => new((int)(vector2.X * vunit.FactorX), (int)(vector2.Y * vunit.FactorY));

    /// <summary>
    /// Used for drawing real scale of images.
    /// </summary>
    public static Pixel VirtualizePixel(this Pixel pixel, Camera camera) => new((int)(pixel.X * camera.Scaler.FactorX), (int)(pixel.Y * camera.Scaler.FactorY));

}