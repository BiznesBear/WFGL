using WFGL.Core;
using WFGL.Rendering;

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

    internal event Action? WhenUpdate;

    private Dictionary<Layer, List<Entity>> Order = [];

    private readonly List<Entity> objects = new();


    

    public Hierarchy(GameMaster m) { ChangedList += UpdateOrder; SetMaster(m); }

    public void Register(Entity entity)
    {
        entity.SetHierarchy(this);

        objects.Add(entity);
        WhenUpdate += entity.OnUpdate;

        AddedObject?.Invoke(entity);
        ChangedList.Invoke();
    }

    public void Deregister(Entity entity)
    {
        entity.SetHierarchy(null);

        WhenUpdate -= entity.OnUpdate;
        objects.Remove(entity);

        RemovedObject?.Invoke(entity);
        ChangedList.Invoke();
    }

    public void DestroyAll()
    {
        foreach (Entity obj in objects)
            obj.Destroy(this);
    }

    public void UpdateOrder() =>
        Order = LayerMaster.SortObjectList(objects);

    public IEnumerable<Entity> GetObjects() => LayerMaster.GetObjectsFrom(GetMaster().LayerMaster, Order);

   

    public override void OnUpdate()
    {
        WhenUpdate?.Invoke();
    }

    public override void OnDraw()
    {
        DrawAll();
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

    public IEnumerable<T> GetAllObjectsWithType<T>()
    {
        foreach (var item in objects)
            if (item is T t) yield return t;
    }
}
