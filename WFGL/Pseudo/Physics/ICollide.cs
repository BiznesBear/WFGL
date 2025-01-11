using WFGL.Core;

namespace WFGL.Pseudo.Physics;
public interface ICollide
{
    public Vec2 ColliderSize { get; }
    public Vec2 ColliderPosition { get; }
}
public interface ICircleCollide
{
    public float ColliderRadious { get; }
    public Vec2 ColliderPosition { get; }
}
public static class Collider
{

    #region IsColliding
    public static bool IsColliding(this ICollide self, ICollide other) =>
            self.ColliderPosition.X < other.ColliderPosition.X + other.ColliderSize.X && other.ColliderPosition.X < self.ColliderPosition.X + self.ColliderSize.X &&
            self.ColliderPosition.Y < other.ColliderPosition.Y + other.ColliderSize.Y && other.ColliderPosition.Y < self.ColliderPosition.Y + self.ColliderSize.Y;

    public static bool IsColliding(this ICollide self, IEnumerable<ICollide> other, out ICollide? collider)
    {
        foreach (ICollide coll in other)
        {
            if (self.IsColliding(coll))
            {
                collider = coll;
                return true;
            }
        }
        collider = null;
        return false;
    }
    public static bool IsColliding(this ICollide self, IEnumerable<ICollide> other) =>
        self.IsColliding(other, out ICollide? c);

    #endregion

    #region IsCircleColliding

    public static bool IsColliding(this ICircleCollide self, ICircleCollide other)
    {
        float dx = self.ColliderPosition.X - other.ColliderPosition.X;
        float dy = self.ColliderPosition.Y - other.ColliderPosition.Y;
        float distanceSquared = dx * dx + dy * dy;

        float radiiSum = self.ColliderRadious + other.ColliderRadious;

        return distanceSquared <= radiiSum * radiiSum;
    }

    public static bool IsColliding(this ICircleCollide self, IEnumerable<ICircleCollide> other) =>
        self.IsColliding(other, out ICircleCollide? c);


    public static bool IsColliding(this ICircleCollide self, IEnumerable<ICircleCollide> other, out ICircleCollide? collider)
    {
        foreach (ICircleCollide coll in other)
        {
            if (self.IsColliding(coll))
            {
                collider = coll;
                return true;
            }
        }
        collider = null;
        return false;
    }

    #endregion

    public static void DrawColliderBounds(this ICollide self, GameMaster m) =>
        m.DrawBox(new(self.ColliderPosition, self.ColliderSize));
}