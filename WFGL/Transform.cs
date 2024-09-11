using WFGL.Core;
namespace WFGL;
public abstract class Transform : IBaseTransform
{
    public string Name { get; set; } = "NewTransform";
    public Vector2 Size { get; set; } = Vector2.One;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Pivot { get; set; } = Vector2.Half;
    public float Rotation { get; set; }
    public Layer Layer { get; set; } = Layer.Defalut;
    public void Create(Hierarchy hierarchy)
    {
        OnCreate();
        hierarchy.Transforms.Add(this);
    }
    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy();
        hierarchy.Transforms.Remove(this);
    }

    public virtual void OnCreate() { }
    public virtual void OnDestroy() { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}

public interface IBaseTransform
{
    public Vector2 Size { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
}