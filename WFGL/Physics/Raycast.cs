using WFGL.Core;
namespace WFGL.Physics;
// TODO: Fix this. It ray's in infinite range. 
public struct Ray(Vector2 origin, Vector2 direction)
{
    public Vector2 Origin { get; set; } = origin;
    public Vector2 Direction { get; set; } = direction;
    public float? maxRange = null;
    public Ray(Vector2 origin, Vector2 direction, float range) : this(origin, direction) { maxRange = range; }
    public void DrawGizmos(GameMaster m)
    {
        m.DrawLine(Origin.ToPoint(m.VirtualScale), Direction.ToPoint(m.VirtualScale));
    }
    public void DrawGizmos(GameMaster m, Vector2 intersectionPoint)
    {
        m.DrawLine(Origin.ToPoint(m.VirtualScale), intersectionPoint.ToPoint(m.VirtualScale));
    }
    public override string ToString() => $"{Origin} => {Direction}";
}
public readonly struct RayInfo
{
    public readonly Ray ray;
    public readonly Vector2 intersectionPoint;
    public readonly bool anyIntersects;
    public RayInfo(Ray r, float t_hit)
    {
        ray = r;
        intersectionPoint = ray.Origin + t_hit * ray.Direction;
        anyIntersects = true;
    }
    public override string ToString() => $"{ray}: {intersectionPoint}";
}