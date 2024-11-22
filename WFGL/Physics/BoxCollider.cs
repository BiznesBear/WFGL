using WFGL.Objects;

namespace WFGL.Physics;

public class BoxCollider : Transform, ICollide
{
    public Box Box { get; set; }

    public BoxCollider() { }
    public BoxCollider(Box box)
    {
        Box = box;
    }
    
    public virtual Vec2 ColliderSize => Box.Size;
    public virtual Vec2 ColliderPosition => Position + Box.Location;
}
