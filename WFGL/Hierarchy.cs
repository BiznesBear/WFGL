using WFGL.Core;

namespace WFGL;

public class Hierarchy 
{
    private GameMaster? Master { get; set; }
    private GameMaster GetMaster => Master ?? throw new GameError("Null master in hierarchy");
    private Dictionary<Layer, List<Transform>> Order = [];

    private readonly List<Transform> Transforms = new();

    public event Action ChangedList;
    public event WFGLTransformEventArgs? Added;
    public event WFGLTransformEventArgs? Removed;

    public Hierarchy() { ChangedList += UpdateOrder; }
    public Hierarchy(GameMaster master) : this() { AssignMaster(master); }
    public void AssignMaster(GameMaster master) => Master = master;

    public void Register(Transform transform)
    {
        Transforms.Add(transform);
        Added?.Invoke(transform);
        ChangedList.Invoke();
        GetMaster.WhenUpdate += transform.OnUpdate;
    }
    public void Unregister(Transform transform)
    {
        GetMaster.WhenUpdate -= transform.OnUpdate;
        Transforms.Remove(transform);
        Removed?.Invoke(transform);
        ChangedList.Invoke();
    }
    public void UpdateOrder()
    {
        Order = Transforms.GroupBy(t => t.Layer).ToDictionary(g => g.Key, g => g.ToList());
    }

    [Obsolete]
    public void UpdateAll() 
    {
        if (Master == null) throw new GameError("Not assigned game master to hierarchy");
        foreach (var trans in Transforms)
        { 
            trans.OnUpdate(Master);
            var iparentable = trans as IParentable;
            iparentable?.UpdateToParent(Master);
        }
    }

    public void DrawAll()
    {
        if (Master == null) throw new GameError("Not assigned game master to hierarchy");
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
        // draw the hierarchy here with draw weight
        //foreach (Layer layer in Layer.List)
        //{
        //    foreach (Transform transform in Transforms)
        //    {
        //        if (transform.Layer != layer) continue;
        //        transform.OnDraw(Master);
        //    }
        //}
    }
}
