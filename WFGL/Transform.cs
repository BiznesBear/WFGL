using WFGL.Core;
namespace WFGL;
public abstract class Transform
{
    public string Name { get; set; } = "NewTransform";
    public Vector2 Size { get; set; } = Vector2.One;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Pivot { get; set; } = new(0.5f,0.5f);
    public float Rotation { get; private set; }
    public Layer Layer { get; set; } = Layer.Defalut;

    public void Create(Hierarchy hierarchy)
    {
        OnCreate();
        hierarchy.Objects.Add(this);
    }
    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy();
        hierarchy.Objects.Remove(this);
    }

    public virtual void OnCreate() { }
    public virtual void OnDestroy() { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}