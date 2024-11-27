using WFGL.Core;
using WFGL.Rendering;
using WFGL.Utilities;

namespace WFGL.Objects;


// TODO: Make hierarchy drawable?
public class Hierarchy : Entity  
{
    public List<Entity> Objects => objects;
    public IReadOnlyList<Entity> Init
    {
        set
        {
            foreach (Entity obj in value)
                obj.Create(this);
        }
    }

    public event Action ChangedList;
    public event EntityEventHandler? AddedObject;
    public event EntityEventHandler? RemovedObject;

    public event Action? Update;

    private Dictionary<Layer, List<Entity>> Order = [];
    private readonly List<Entity> objects = [];

    public Hierarchy(GameMaster m) 
    { 
        ChangedList += UpdateOrder; 
        SetMaster(m); 
    }

    public void Register(Entity entity)
    {
        entity.SetHierarchy(this);

        objects.Add(entity);
        Update += entity.OnUpdate;

        AddedObject?.Invoke(entity);
        ChangedList.Invoke();

        Wrint.Register($"{entity.GetType().Name} in {GetType().Name}");
    }

    public void Deregister(Entity entity)
    {
        entity.SetHierarchy(null);

        Update -= entity.OnUpdate;
        objects.Remove(entity);

        RemovedObject?.Invoke(entity);
        ChangedList.Invoke();
        Wrint.Deregister($"{entity.GetType().Name} in {GetType().Name}");
    }

    public void DestroyAll()
    {
        foreach (Entity obj in objects)
            obj.Destroy(this);
        Wrint.Info("Destroyed all objects");
    }

    public void UpdateOrder() =>
        Order = LayerMaster.SortObjectList(objects);

    public IEnumerable<Entity> GetObjects() => LayerMaster.GetObjectsFrom(GetMaster().LayerMaster, Order);

   
    public override void OnUpdate()
    {
        base.OnUpdate();
        Update?.Invoke();
    }
    public override void OnDraw()
    {
        base.OnDraw();
        DrawAll();
    }

    public IEnumerable<T> GetAllObjectsWithType<T>()
    {
        foreach (var item in objects)
            if (item is T t) yield return t;
    }
    

    protected void DrawAll()
    {
        foreach (Entity entity in GetObjects())
        {
            entity.OnDraw();
            OnEntityDraw(entity);
        }
    }
    protected virtual void OnEntityDraw(Entity entity)
    {
        if (entity is IDrawable d) 
            d.Draw(GetMaster(), GetMaster().Renderer);
    }
}
