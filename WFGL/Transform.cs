using WFGL.Core;
namespace WFGL;

public abstract class Transform : IObject
{
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Rotation { get; set; }
    public Layer Layer { get; set; } = Layer.Defalut;

    public void Create(Hierarchy hierarchy)
    {
        hierarchy.Register(this);
        OnCreate();
    }
    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy();
        hierarchy.Unregister(this);
    }
    public virtual void OnCreate() { }
    public virtual void OnDestroy() { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}
