using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

/// <summary>
/// Base object without any additional properties.
/// </summary>
public abstract class Entity  
{
    private Hierarchy? Hierarchy { get; set; }
    private GameMaster? Master { get; set; }

    public Layer Layer { get; set; } = Layer.Defalut;

    public GameMaster GetMaster() => Master ?? throw new ArgumentNullException("Null game master instance in entity");
    public Hierarchy GetHierarchy() => Hierarchy ?? throw new ArgumentNullException("Null hierarchy instance in entity");
        
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
    public virtual void OnCreate(Hierarchy h, GameMaster m) { Master = m; }
    public virtual void OnDestroy(Hierarchy h, GameMaster m) { }
    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }
}