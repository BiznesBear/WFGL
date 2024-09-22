using WFGL.Core;

namespace WFGL;

public class Hierarchy 
{
    private GameMaster? Master { get; set; }
    private GameMaster GetMaster => Master ?? throw new WFGLError("Null master in hierarchy");
    private Dictionary<Layer, List<Transform>> Order = [];

    private readonly List<Transform> Transforms = new();

    public event Action ChangedList;
    public event WFGLTransformEventArgs? AddedTransform;
    public event WFGLTransformEventArgs? RemovedTransform;

    public List<Transform> Registration
    {
        set
        {
            foreach(Transform t in value)
            {
                t.Create(this);
            }
        }
    }

    public Hierarchy() { ChangedList += UpdateOrder; }
    public Hierarchy(GameMaster master) : this() { AssignMaster(master); }
    public void AssignMaster(GameMaster master) => Master = master;

    public void Register(Transform transform)
    {
        Transforms.Add(transform);
        AddedTransform?.Invoke(transform);
        ChangedList.Invoke();
        GetMaster.WhenUpdate += transform.OnUpdate;
    }
    public void Unregister(Transform transform)
    {
        GetMaster.WhenUpdate -= transform.OnUpdate;
        Transforms.Remove(transform);
        RemovedTransform?.Invoke(transform);
        ChangedList.Invoke();
    }
    public void UpdateOrder()
    {
        Order = Transforms.GroupBy(t => t.Layer).ToDictionary(g => g.Key, g => g.ToList());
    }

    public void DrawAll()
    {
        if (Master == null) throw new WFGLError("Not assigned game master to hierarchy");
        foreach (Layer layer in Layer.List)
        {
            if (Order.TryGetValue(layer, out var transformsForLayer))
            {
                foreach (Transform transform in transformsForLayer)
                {
                    transform.OnDraw(GetMaster);
                }
            }
        }
    }
}
