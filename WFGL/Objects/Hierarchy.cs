using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

public class Hierarchy : Entity
{
    private Dictionary<Layer, List<Entity>> Order = [];

    private readonly List<Entity> objects = new();

    public event Action ChangedList;
    public event EntityEventHandler? AddedObject;
    public event EntityEventHandler? RemovedObject;

    internal event GameMasterEventHandler? WhenUpdate;

    public List<Entity> Objects
    {
        get => objects;
        set
        {
            foreach (Entity obj in value)
                obj.Create(this);
        }
    }

    public Hierarchy(GameMaster m) { ChangedList += UpdateOrder; Master = m; }

    public void Register(Entity entity)
    {
        entity.SetHierarchy(this);
        objects.Add(entity);
        AddedObject?.Invoke(entity);
        ChangedList.Invoke();
        WhenUpdate += entity.OnUpdate;
    }

    public void Deregister(Entity entity)
    {
        entity.SetHierarchy(null);
        WhenUpdate -= entity.OnUpdate;
        objects.Remove(entity);
        RemovedObject?.Invoke(entity);
        ChangedList.Invoke();
    }


    public void UpdateOrder() =>
        Order = LayerMaster.SortObjectList(objects);

    public IEnumerable<Entity> GetObjects() => LayerMaster.GetObjectsFrom(GetMaster().LayerMaster, Order);

    public void DestroyAll()
    {
        foreach (Entity obj in objects) 
            obj.Destroy(this);
    }

    public override void OnUpdate(GameMaster m)
    {
        WhenUpdate?.Invoke(m);
    }

    public override void OnDraw(GameMaster m)
    {
        DrawAll();
    }

    protected void DrawAll()
    {
        foreach (Entity entity in GetObjects())
        {
            entity.OnDraw(GetMaster());
            if (entity is IDrawable d)
                d.Draw(GetMaster(), GetMaster().Renderer);
        }
    }

    public IEnumerable<T> GetAllObjectsWithType<T>()
    {
        foreach (var item in objects)
            if (item is T t) yield return t;
    }
}
