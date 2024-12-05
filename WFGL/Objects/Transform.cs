using WFGL.Core;
using WFGL.Physics;
namespace WFGL.Objects;

public abstract class TransformBase<T> : Entity where T : struct, IVec<T>
{
    public virtual T Scale { get; set; } 
    public T Position { get; set; }
}
public abstract class Transform : TransformBase<Vec2>
{
    public override Vec2 Scale { get; set; } = Vec2.One;
    public Point RealPosition 
    { 
        get => Position.ToPoint(Master.VirtualScale); 
        set => Position = value.ToVec2(Master.VirtualScale); 
    }
}
public abstract class Transform3D : TransformBase<Vec3>
{
    public override Vec3 Scale { get; set; } = Vec3.One;
    public Vec3 Rot { get; set; } = new(0, 0, 0);
    public Point RealPosition
    {
        get => Position.ToPoint(Master.VirtualScale);
        set => Position = value.ToVec3(Master.VirtualScale);
    }
}