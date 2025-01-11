using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

/// <summary>
/// Defalut object for components
/// </summary>
public abstract class Entity
{
    public Layer Layer { get; set; } = Layer.Defalut;

    private Hierarchy? hierarchy;
    private GameMaster? master;
    
    public GameMaster Master => master ?? throw new NullReferenceException("Null GameMaster instance in entity");
    public Hierarchy Hierarchy => hierarchy ?? throw new NullReferenceException("Null Hierarchy instance in entity");

    public void SetMaster(GameMaster master) => this.master = master; 
    public void SetHierarchy(Hierarchy? hierarchy) => this.hierarchy = hierarchy;

    public void Create(Hierarchy hierarchy)
    {
        hierarchy.Register(this);
        OnCreate(hierarchy, hierarchy.Master);
    }

    public void Destroy(Hierarchy hierarchy)
    {
        OnDestroy(hierarchy, hierarchy.Master);
        hierarchy.Deregister(this);
    }
    public virtual void OnCreate(Hierarchy h, GameMaster m) { SetMaster(m); }
    public virtual void OnDestroy(Hierarchy h, GameMaster m) { }
    public virtual void OnUpdate() { }
    public virtual void OnDraw() { }
}