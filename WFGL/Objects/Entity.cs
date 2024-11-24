using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

/// <summary>
/// Base class for components.
/// </summary>
public abstract class Entity  
{
    /// <summary>
    /// Optional entity name only used in WFGE for idenifing entities.
    /// </summary>
    public string Name => GetType().Name;
    private Hierarchy? Hierarchy { get; set; }
    private GameMaster? Master { get; set; }

    public Layer Layer { get; set; } = Layer.Defalut;

    public GameMaster GetMaster() => Master ?? throw new ArgumentNullException("Null GameMaster instance in entity");
    public Hierarchy GetHierarchy() => Hierarchy ?? throw new ArgumentNullException("Null Hierarchy instance in entity");

    public void SetMaster(GameMaster master) => Master = master; 
    public void SetHierarchy(Hierarchy? hierarchy) => Hierarchy = hierarchy;

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
    public virtual void OnCreate(Hierarchy h, GameMaster m) { SetMaster(m); }
    public virtual void OnDestroy(Hierarchy h, GameMaster m) { }
    public virtual void OnUpdate() { }
    public virtual void OnDraw() { }
}