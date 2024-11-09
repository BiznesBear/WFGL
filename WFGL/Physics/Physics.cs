using WFGL.Rendering;

namespace WFGL.Physics;

public static class Physics
{
    public static bool IsColliding(this ICollide self, ICollide other)
    {
        if (self == other) return false; // avoid colliding with yourself
        float leftA = self.ColliderPosition.X;
        float rightA = self.ColliderPosition.X + self.ColliderSize.X;
        float topA = self.ColliderPosition.Y;
        float bottomA = self.ColliderPosition.Y + self.ColliderSize.Y;


        float leftB = other.ColliderPosition.X;
        float rightB = other.ColliderPosition.X + other.ColliderSize.X;
        float topB = other.ColliderPosition.Y;
        float bottomB = other.ColliderPosition.Y + other.ColliderSize.Y;

        if (rightA < leftB || leftA > rightB || bottomA < topB || topA > bottomB) 
            return false;

        return true;
    }
    public static bool IsColliding(this ICollide self, IEnumerable<ICollide> other, out ICollide? collider)
    {
        foreach (ICollide coll in other)
            if (IsColliding(self, coll)) 
            { 
                collider = coll;
                return true; 
            }
        collider = null;
        return false;
    }

    public static bool IsColliding(this Ray ray, ICollide coll, out RaycastInfo info)
    {
        info = new();

        float left = coll.ColliderPosition.X;
        float right = coll.ColliderPosition.X + coll.ColliderSize.X;
        float top = coll.ColliderPosition.Y;
        float bottom = coll.ColliderPosition.Y + coll.ColliderSize.Y;

        float t_min = (left - ray.Origin.X) / ray.Direction.X;
        float t_max = (right - ray.Origin.X) / ray.Direction.X;

        if (t_min > t_max)
            (t_max, t_min) = (t_min, t_max);

        float t_ymin = (top - ray.Origin.Y) / ray.Direction.Y;
        float t_ymax = (bottom - ray.Origin.Y) / ray.Direction.Y;

        if (t_ymin > t_ymax)
            (t_ymax, t_ymin) = (t_ymin, t_ymax);

        if (t_min > t_ymax || t_ymin > t_max) return false;

        t_min = Math.Max(t_min, t_ymin);
        t_max = Math.Min(t_max, t_ymax);

        if (t_max < 0) return false;

        float t_hit = (t_min >= 0) ? t_min : t_max;

        if (ray.maxRange != null && t_hit > ray.maxRange)
            return false;

        info = new(ray,t_hit);

        return true; 
    }
    public static void DrawColliderBounds(this ICollide self, Core.GameMaster m)
    {
        m.DrawRect(new(self.ColliderPosition.ToPoint(m), self.ColliderSize.ToSize(m)));
    }
}
