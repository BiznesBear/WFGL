using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Physics;
/// <summary>
/// Ray used for calculating raycasts
/// </summary>
/// <param name="origin">Start position of ray</param>
/// <param name="direction">Look direction of ray</param>
public struct Ray(Vec2 origin, Vec2 direction)
{
    public Vec2 Origin { get; set; } = origin;
    public Vec2 Direction { get; set; } = direction;
    public float? maxRange = null;
    public Ray(Vec2 origin, Vec2 direction, float range) : this(origin, direction) { maxRange = range; }


    public readonly bool IsColliding(ICollide coll, out RaycastInfo info)
    {
        info = new();

        float left = coll.ColliderPosition.X;
        float right = coll.ColliderPosition.X + coll.ColliderSize.X;
        float top = coll.ColliderPosition.Y;
        float bottom = coll.ColliderPosition.Y + coll.ColliderSize.Y;

        float t_min = (left - Origin.X) / Direction.X;
        float t_max = (right - Origin.X) / Direction.X;

        if (t_min > t_max)
            (t_max, t_min) = (t_min, t_max);

        float t_ymin = (top - Origin.Y) / Direction.Y;
        float t_ymax = (bottom - Origin.Y) / Direction.Y;

        if (t_ymin > t_ymax)
            (t_ymax, t_ymin) = (t_ymin, t_ymax);

        if (t_min > t_ymax || t_ymin > t_max) return false;

        t_min = Math.Max(t_min, t_ymin);
        t_max = Math.Min(t_max, t_ymax);

        if (t_max < 0) return false;

        float t_hit = (t_min >= 0) ? t_min : t_max;

        if (t_hit > maxRange)
            return false;

        info = new(this, t_hit);

        return true;
    }

    public void DrawGizmos(GameMaster m) =>
        m.DrawLine(Origin.ToPoint(m), Direction.ToPoint(m));

    public void DrawGizmos(GameMaster m, Vec2 intersectionPoint) =>
        m.DrawLine(Origin.ToPoint(m), intersectionPoint.ToPoint(m));

    public override string ToString() => $"{Origin} => {Direction}";
}

public readonly struct RaycastInfo
{
    public readonly Ray ray;
    public readonly Vec2 collisionPoint;
    public readonly bool isColliding;
    public RaycastInfo(Ray r, float t_hit)
    {
        ray = r;
        collisionPoint = ray.Origin + t_hit * ray.Direction;
        isColliding = true;
    }
    public override string ToString() => $"{ray}: {collisionPoint}";
}