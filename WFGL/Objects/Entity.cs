using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

public interface IObject
{
    public Layer Layer { get; set; }
    public void Create(Hierarchy hierarchy);
    public void Destroy(Hierarchy hierarchy);

    public void OnUpdate(GameMaster m);
    public void OnDraw(GameMaster m);
}

/// <summary>
/// Object without any properties. 
/// </summary>
public abstract class Entity : IObject 
{
    public Layer Layer { get; set; } = Layer.Defalut;

    protected GameMaster? Master { get; set; }
    public GameMaster GetMaster() => Master ?? throw new ArgumentNullException("Null game master instance in transform");
    public void SetMaster(GameMaster master) => Master = master;

    public void Create(Hierarchy hierarchy)
    {
        hierarchy.Register(this);
        OnCreate(hierarchy, hierarchy.GetMaster());
    }
    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy(hierarchy, hierarchy.GetMaster());
        hierarchy.Deregister(this);
    }
    public virtual void OnCreate(Hierarchy h, GameMaster m) { Master = m; }
    public virtual void OnDestroy(Hierarchy h, GameMaster m) { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}