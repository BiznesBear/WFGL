using WFGL.Core;

namespace WFGL;

public class Hierarchy : Transform
{
    private GameMaster? Master { get; set; }
    public GameMaster GetMaster() => Master ?? throw new WFGLNullInstanceError("Null master in hierarchy");
    private Dictionary<Layer, List<IObject>> Order = [];

    private readonly List<IObject> objects = new();

    public event Action ChangedList;
    public event ObjectEventArgs? AddedObject;
    public event ObjectEventArgs? RemovedObject;

    public List<IObject> Objects
    {
        get => objects;
        set
        {
            foreach(IObject obj in value) 
                obj.Create(this);
        }
    }

    public Hierarchy() { ChangedList += UpdateOrder; }
    public Hierarchy(GameMaster master) : this() { AssignMaster(master); }
    public void AssignMaster(GameMaster master) => Master = master;

    public void Register(IObject obj)
    {
        objects.Add(obj);
        AddedObject?.Invoke(obj);
        ChangedList.Invoke();
        GetMaster().WhenUpdate += obj.OnUpdate;
    }
    public void Unregister(IObject obj)
    {
        GetMaster().WhenUpdate -= obj.OnUpdate;
        objects.Remove(obj);
        RemovedObject?.Invoke(obj);
        ChangedList.Invoke();
    }
    public void UpdateOrder()
    {
        Order = Layer.SortObjectList(objects);
    }
    public IEnumerable<IObject> GetObjects() => Layer.GetObjectsFrom(Order);
    public void DrawAll()
    {
        foreach (IObject obj in GetObjects())
        {
            obj.OnDraw(GetMaster());
        }
    }
}
