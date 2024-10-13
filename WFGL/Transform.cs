using WFGL.Core;
namespace WFGL;
public interface IObject 
{
    public Layer Layer { get; set; }
    public void Create(Hierarchy hierarchy);
    public void Destroy(Hierarchy hierarchy);

    public void OnUpdate(GameMaster m);
    public void OnDraw(GameMaster m);
}

public abstract class TransformBase<T> : IObject  where T : struct, IVec<T>
{
    public virtual T Scale { get; set; } = default;
    public T Position { get; set; } = default;

    public Layer Layer { get; set; } = Layer.Defalut;

    protected GameMaster? master;
    public GameMaster GetMaster() => master ?? throw new WFGLNullInstanceError("Null game master instance in transform");

    public void Create(Hierarchy hierarchy)
    {
        hierarchy.Register(this);
        OnCreate(hierarchy, hierarchy.GetMaster());
    }
    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy(hierarchy, hierarchy.GetMaster());
        hierarchy.Unregister(this);
    }
    public virtual void OnCreate(Hierarchy h, GameMaster m) { master = m; }
    public virtual void OnDestroy(Hierarchy h, GameMaster m) { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}
public abstract class Transform : TransformBase<Vec2>
{
    public override Vec2 Scale { get; set; } = Vec2.One;
    public Point RealPosition => Position.ToPoint(GetMaster());
}