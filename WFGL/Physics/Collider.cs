namespace WFGL.Physics;
public interface ICollide
{
    public Vector2 ColliderSize { get; }
    public Vector2 ColliderPosition { get; }
}
public class Collider : Transform, ICollide
{
    public Vector2 colliderSize = Vector2.One;
    public Vector2 colliderOffset = Vector2.Zero;
    public virtual Vector2 ColliderSize => colliderSize;
    public virtual Vector2 ColliderPosition => Position + colliderOffset;
}
