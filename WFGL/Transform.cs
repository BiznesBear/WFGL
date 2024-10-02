using WFGL.Core;
namespace WFGL;

public abstract class Transform : IObject
{
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Rotation { get; set; }
    public Layer Layer { get; set; } = Layer.Defalut;

    protected GameMaster? master;
    protected GameMaster GetMaster() => master ?? throw new WFGLNullInstanceError("Null game master instance in transform");

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
