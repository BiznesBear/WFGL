using WFGL.Core;
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
    #region Vec2
    #region Push

    public static Point PushToPoint(this Size size) => new(size.Width, size.Height);
    public static Size PushToSize(this Point pixel) => new(pixel.X, pixel.Y);
    public static Rectangle PushToRect(this Point pixel) => new(0,0, pixel.X, pixel.Y);

    #endregion

    #region Converts
    public static Vec2 ToVector2(this Point point, GameMaster m) => new(point.X / m.VirtualScale.FactorX, point.Y / m.VirtualScale.FactorY);
    public static Vec2 ToVector2(this Size size, GameMaster m) => size.PushToPoint().ToVector2(m);
    public static Point ToPoint(this Vec2 vector2, GameMaster m) => new((int)(vector2.X * m.VirtualScale.FactorX), (int)(vector2.Y * m.VirtualScale.FactorY));
    public static Size ToSize(this Vec2 vector2, GameMaster m) => vector2.ToPoint(m).PushToSize();
    #endregion
    #endregion

    #region Vec3



    public static Vec3 ToVector3(this Point point, GameMaster m) => new(point.X / m.VirtualScale.FactorX, point.Y / m.VirtualScale.FactorY, 0); //TODO: Here add persepctive
    public static Vec3 ToVector3(this Size size, GameMaster m) => size.PushToPoint().ToVector3(m);
    public static Point ToPoint(this Vec3 vector3, GameMaster m) => new((int)(vector3.X * m.VirtualScale.FactorX), (int)(vector3.Y * m.VirtualScale.FactorY));
    public static Size ToSize(this Vec3 vector3, GameMaster m) => vector3.ToPoint(m).PushToSize();

    #endregion


    #region Virtualization
    /// <summary>
    /// Used for rescaling 
    /// </summary>
    public static Point VirtualizePixel(this Point pixel, Camera camera) => new((int)(pixel.X * camera.Scaler), (int)(pixel.Y * camera.Scaler));

    /// <summary>
    /// 
    /// </summary>
    public static Size VirtualizePixel(this Size pixel, Camera camera) => pixel.PushToPoint().VirtualizePixel(camera).PushToSize();
    #endregion
}