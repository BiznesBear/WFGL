using WFGL.Core;

namespace WFGL.Physics;
public class Collider : Transform, ICollide
{
    public Vector2 colliderSize = Vector2.One;
    public Vector2 colliderOffset = Vector2.Zero;
    public Vector2 ColliderSize => colliderSize;
    public Vector2 ColliderPosition => Position + colliderOffset;

    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
    }   
}
