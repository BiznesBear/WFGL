using System.Numerics;
using WFGL.Physics;
using WFGL.Rendering;

namespace WFGL.Core;

public static class Converts
{
    // basic unit converts
    public static Point ToPoint(this Size size) => new(size.Width, size.Height);
    public static Size ToSize(this Point pixel) => new(pixel.X, pixel.Y);

    // point
    public static Point ToPoint(this Vec2 vec2, VirtualUnit u) => new((int)(vec2.X * u.FactorX), (int)(vec2.Y * u.FactorY));
    public static Point ToPoint(this Vec3 vec3, VirtualUnit u) => new((int)(vec3.X * u.FactorX), (int)(vec3.Y * u.FactorY));

    // size
    public static Size ToSize(this Vec2 vec2, VirtualUnit u) => vec2.ToPoint(u).ToSize();
    public static Size ToSize(this Vec3 vec3, VirtualUnit u) => vec3.ToPoint(u).ToSize();

    // vec2
    public static Vec2 ToVec2(this Point point, VirtualUnit u) => new(point.X / u.FactorX, point.Y / u.FactorY);
    public static Vec2 ToVec2(this Size size, VirtualUnit u) => size.ToPoint().ToVec2(u);
    public static Vec2 ToVec2(this Vector2 vector2) => new(vector2.X, vector2.Y);


    // vec3
    public static Vec3 ToVec3(this Point point, VirtualUnit u) => new(point.X / u.FactorX, point.Y / u.FactorY, 0);
    public static Vec3 ToVec3(this Size size, VirtualUnit u) => size.ToPoint().ToVec3(u);
    public static Vec3 ToVec3(this Vector3 vector3) => new(vector3.X, vector3.Y, vector3.Z);


    // vector 
    public static Vector2 ToVector2(this Vec2 vec2) => new(vec2.X, vec2.Y);
    public static Vector3 ToVector3(this Vec3 vec3) => new(vec3.X, vec3.Y, vec3.Z);

}
public record Pixel(int X,int Y);
public static class Virtualization
{
    /// <summary>
    /// Scales pixel by view scaler.
    /// </summary>
    public static Pixel VirtualizePixel(this Pixel pixel, View view) => new((int)(pixel.X * view.Scaler), (int)(pixel.Y * view.Scaler));

    /// <summary>
    /// Scales point by view scaler.
    /// </summary>
    public static Point VirtualizePixel(this Point pixel, View view)
    {
        Pixel p = VirtualizePixel(new Pixel(pixel.X, pixel.Y), view);
        return new Point(p.X, p.Y);
    }

    /// <summary>
    /// Scales size by view scaler.
    /// </summary>
    public static Size VirtualizePixel(this Size pixel, View view)
    {
        Pixel p = VirtualizePixel(new Pixel(pixel.Width, pixel.Height), view);
        return new Size(p.X, p.Y);
    }
}