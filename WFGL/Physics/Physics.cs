namespace WFGL.Physics;

public static class Physics
{
    public static bool IsColliding(this ICollide self, ICollide other)
    {
        float leftA = self.ColliderPosition.X;
        float rightA = self.ColliderPosition.X + self.ColliderSize.X;
        float topA = self.ColliderPosition.Y;
        float bottomA = self.ColliderPosition.Y + self.ColliderSize.Y;


        float leftB = other.ColliderPosition.X;
        float rightB = other.ColliderPosition.X + other.ColliderSize.X;
        float topB = other.ColliderPosition.Y;
        float bottomB = other.ColliderPosition.Y + other.ColliderSize.Y;

        if (rightA < leftB || leftA > rightB || bottomA < topB || topA > bottomB) return false;
        return true;
    }


    public static bool IsColliding(this Ray ray, ICollide rect, out RayInfo info)
    {
        info = new();

        float left = rect.ColliderPosition.X;
        float right = rect.ColliderPosition.X + rect.ColliderSize.X;
        float top = rect.ColliderPosition.Y;
        float bottom = rect.ColliderPosition.Y + rect.ColliderSize.Y;

        float t_min = (left - ray.Origin.X) / ray.Direction.X;
        float t_max = (right - ray.Origin.X) / ray.Direction.X;

        if (t_min > t_max)
        {
            (t_max, t_min) = (t_min, t_max);
        }

        float t_ymin = (top - ray.Origin.Y) / ray.Direction.Y;
        float t_ymax = (bottom - ray.Origin.Y) / ray.Direction.Y;

        if (t_ymin > t_ymax)
        {
            (t_ymax, t_ymin) = (t_ymin, t_ymax);
        }

        if (t_min > t_ymax || t_ymin > t_max) return false;

        t_min = Math.Max(t_min, t_ymin);
        t_max = Math.Min(t_max, t_ymax);

        if (t_max < 0) return false;

        float t_hit = (t_min >= 0) ? t_min : t_max;
        
        info = new(ray,t_hit);

        return true; 
    }
    public static void DrawColliderBounds(this ICollide self, Core.GameMaster m)
    {
        m.DrawRect(self.ColliderPosition.ToPoint(m.VirtualScale), self.ColliderSize.ToPoint(m.VirtualScale));
    }
}

public struct Ray(Vector2 origin, Vector2 direction)
{
    public Vector2 Origin { get; set; } = origin;  
    public Vector2 Direction { get; set; } = direction;

    public void DrawGizmos(Core.GameMaster m)
    {
        m.DrawLine(Origin.ToPoint(m.VirtualScale), Direction.ToPoint(m.VirtualScale));
    }
    public void DrawGizmos(Core.GameMaster m, Vector2 intersectionPoint)
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
        intersectionPoint = new(
            ray.Origin.X + t_hit * ray.Direction.X,
            ray.Origin.Y + t_hit * ray.Direction.Y
        );
        anyIntersects = true;
    }
    public override string ToString() => $"{ray}: {intersectionPoint}";
}