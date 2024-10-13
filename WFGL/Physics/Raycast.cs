using WFGL.Core;
namespace WFGL.Physics;
// TODO: Fix this. It ray's in infinite range. 
public struct Ray(Vec2 origin, Vec2 direction)
{
    public Vec2 Origin { get; set; } = origin;
    public Vec2 Direction { get; set; } = direction;
    public float? maxRange = null;
    public Ray(Vec2 origin, Vec2 direction, float range) : this(origin, direction) { maxRange = range; }
    public void DrawGizmos(GameMaster m)
    {
        m.DrawLine(Origin.ToPoint(m), Direction.ToPoint(m));
    }
    public void DrawGizmos(GameMaster m, Vec2 intersectionPoint)
    {
        m.DrawLine(Origin.ToPoint(m), intersectionPoint.ToPoint(m));
    }
    public override string ToString() => $"{Origin} => {Direction}";
}
public readonly struct RayInfo
{
    public readonly Ray ray;
    public readonly Vec2 intersectionPoint;
    public readonly bool anyIntersects;
    public RayInfo(Ray r, float t_hit)
    {
        ray = r;
        intersectionPoint = ray.Origin + t_hit * ray.Direction;
        anyIntersects = true;
    }
    public override string ToString() => $"{ray}: {intersectionPoint}";
}