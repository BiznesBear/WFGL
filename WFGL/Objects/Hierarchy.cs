using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Objects;

public class Hierarchy : Transform
{
    private Dictionary<Layer, List<IObject>> Order = [];

    private readonly List<IObject> objects = new();

    public event Action ChangedList;
    public event ObjectEventHandler? AddedObject;
    public event ObjectEventHandler? RemovedObject;
    internal event GameMasterEventHandler? WhenUpdate;

    public List<IObject> Objects
    {
        get => objects;
        set
        {
            foreach (IObject obj in value)
            {
                obj.Create(this);
            }
        }
    }

    public Hierarchy(GameMaster m) { ChangedList += UpdateOrder; master = m; }

    public void Register(IObject obj)
    {
        objects.Add(obj);
        AddedObject?.Invoke(obj);
        ChangedList.Invoke();
        WhenUpdate += obj.OnUpdate;
    }
    public void Deregister(IObject obj)
    {
        WhenUpdate -= obj.OnUpdate;
        objects.Remove(obj);
        RemovedObject?.Invoke(obj);
        ChangedList.Invoke();
    }
    public void UpdateOrder()
    {
        Order = LayerMaster.SortObjectList(objects);
    }
    public IEnumerable<IObject> GetObjects() => LayerMaster.GetObjectsFrom(GetMaster().LayerMaster, Order);
    public void DestroyAll()
    {
        foreach (IObject obj in objects) obj.Destroy(this);
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
        foreach (IObject obj in GetObjects())
        {
            obj.OnDraw(GetMaster());
            if (obj is IDrawable d)
                d.Draw(GetMaster(), GetMaster().Renderer);
        }
    }
    public IEnumerable<T> GetAllObjectsWithType<T>()
    {
        foreach (var item in objects)
            if (item is T t) yield return t;
    }
}
