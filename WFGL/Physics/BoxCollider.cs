using WFGL.Objects;

namespace WFGL.Physics;

public class BoxCollider : Transform, ICollide
{
    public Vec2 colliderSize = Vec2.One;
    public Vec2 colliderOffset = Vec2.Zero;
    public virtual Vec2 ColliderSize => colliderSize;
    public virtual Vec2 ColliderPosition => Position + colliderOffset;
}
