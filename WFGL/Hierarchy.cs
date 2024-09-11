using WFGL.Core;

namespace WFGL;

public class Hierarchy 
{
    private GameMaster? Master { get; set; }
    internal readonly List<Transform> Transforms = new();
    public Hierarchy() { }
    public Hierarchy(GameMaster master) { AssignMaster(master); }
    public void AssignMaster(GameMaster master) => Master = master;

    public void UpdateAll() 
    {
        if (Master == null) throw new Exception("Not assigned game master to hierarchy");
        foreach (var trans in Transforms) trans.OnUpdate(Master);
    }

    public void DrawAll()
    {
        if (Master == null) throw new Exception("Not assigned game master to hierarchy");

        // draw the hierarchy here with draw weight
        foreach (Layer layer in Layer.List)
        {
            foreach (Transform transform in Transforms)
            {
                if (transform.Layer != layer) continue;
                transform.OnDraw(Master);
            }
        }
    }
}
