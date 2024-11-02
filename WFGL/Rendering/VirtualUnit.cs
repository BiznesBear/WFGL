using System.Numerics;
using WFGL.Core;
using WFGL.Physics;
namespace WFGL.Rendering;

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


    public static implicit operator VirtualUnit(float a) => new(a, a);
    public static VirtualUnit operator +(VirtualUnit a, VirtualUnit b) => new(a.FactorX + b.FactorX, a.FactorY + b.FactorY);
    public static VirtualUnit operator -(VirtualUnit a, VirtualUnit b) => new(a.FactorX - b.FactorX, a.FactorY - b.FactorY);
    public static VirtualUnit operator *(VirtualUnit a, VirtualUnit b) => new(a.FactorX * b.FactorX, a.FactorY * b.FactorY);
    public static VirtualUnit operator /(VirtualUnit a, VirtualUnit b) => new(a.FactorX / b.FactorX, a.FactorY / b.FactorY);
    public static VirtualUnit operator %(VirtualUnit a, VirtualUnit b) => new(a.FactorX % b.FactorX, a.FactorY % b.FactorY);
    public readonly override string ToString() => $"VU({FactorX};{FactorY})";

}
public static class Converts
{
    #region Push

    public static Point PushToPoint(this Size size) => new(size.Width, size.Height);
    public static Size PushToSize(this Point pixel) => new(pixel.X, pixel.Y);
    public static Rectangle PushToRect(this Point pixel) => new(0, 0, pixel.X, pixel.Y);

    #endregion

    #region Vec2


    public static Vec2 ToVec2(this Point point, GameMaster m) => new(point.X / m.VirtualScale.FactorX, point.Y / m.VirtualScale.FactorY);
    public static Vec2 ToVec2(this Size size, GameMaster m) => size.PushToPoint().ToVec2(m);
    public static Point ToPoint(this Vec2 vec2, GameMaster m) => new((int)(vec2.X * m.VirtualScale.FactorX), (int)(vec2.Y * m.VirtualScale.FactorY));
    public static Size ToSize(this Vec2 vec2, GameMaster m) => vec2.ToPoint(m).PushToSize();

    // system.numerics vectors

    public static Vec2 ToVec2(this Vector2 vector2) => new(vector2.X, vector2.Y);
    public static Vector2 ToVector2(this Vec2 vec2) => new(vec2.X, vec2.Y);

    #endregion

    #region Vec3

    public static Vec3 ToVec3(this Point point, GameMaster m) => new(point.X / m.VirtualScale.FactorX, point.Y / m.VirtualScale.FactorY, 0);
    public static Vec3 ToVec3(this Size size, GameMaster m) => size.PushToPoint().ToVec3(m);
    public static Point ToPoint(this Vec3 vec3, GameMaster m) => new((int)(vec3.X * m.VirtualScale.FactorX), (int)(vec3.Y * m.VirtualScale.FactorY));
    public static Size ToSize(this Vec3 vec3, GameMaster m) => vec3.ToPoint(m).PushToSize();

    // system.numerics vectors

    public static Vec3 ToVec3(this Vector3 vector3) => new(vector3.X, vector3.Y, vector3.Z);
    public static Vector3 ToVector3(this Vec3 vec3) => new(vec3.X, vec3.Y, vec3.Z);

    #endregion

    #region Virtualization
    /// <summary>
    /// Final scale for objects 
    /// </summary>
    public static Point VirtualizePixel(this Point pixel, Camera camera) => new((int)(pixel.X * camera.Scaler), (int)(pixel.Y * camera.Scaler));
    public static PointF VirtualizePixel(this PointF pixel, Camera camera) => new(pixel.X * camera.Scaler, pixel.Y * camera.Scaler);

    /// <summary>
    /// Final scale for objects 
    /// </summary>
    public static Size VirtualizePixel(this Size pixel, Camera camera) => pixel.PushToPoint().VirtualizePixel(camera).PushToSize();
    #endregion
}