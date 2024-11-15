using WFGL.Core;

namespace WFGL.Physics;
public interface ICollide
{
    public Vec2 ColliderSize { get; }
    public Vec2 ColliderPosition { get; }
}
public static class Collider
{
    public static bool IsColliding(this ICollide self, ICollide other) =>
            (self.ColliderPosition.X < other.ColliderPosition.X + other.ColliderSize.X) && (other.ColliderPosition.X < self.ColliderPosition.X + self.ColliderSize.X) &&
            (self.ColliderPosition.Y < other.ColliderPosition.Y + other.ColliderSize.Y) && (other.ColliderPosition.Y < self.ColliderPosition.Y + self.ColliderSize.Y);

    public static bool IsColliding(this ICollide self, IEnumerable<ICollide> other, out ICollide? collider)
    {
        foreach (ICollide coll in other)
        {
            if (IsColliding(self, coll))
            {
                collider = coll;
                return true;
            }
        }
        collider = null;
        return false;
    }
    public static bool IsColliding(this ICollide self, IEnumerable<ICollide> other) =>
        IsColliding(self, other, out ICollide? c);

    public static void DrawColliderBounds(this ICollide self, GameMaster m) =>
        m.DrawRect(new(self.ColliderPosition.ToPoint(m.VirtualScale), self.ColliderSize.ToSize(m.VirtualScale)));
}