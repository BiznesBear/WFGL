using WFGL.Physics;
using WFGL.Rendering;
namespace WFGL.Objects;

public abstract class TransformBase<T> : Entity where T : struct, IVec<T>
{
    public virtual T Scale { get; set; } 
    public T Position { get; set; }
}
public abstract class Transform : TransformBase<Vec2>
{
    public override Vec2 Scale { get; set; } = Vec2.One;
    public Point RealPosition => Position.ToPoint(GetMaster());
}
