using WFGL.Core;

namespace WFGL;

public class Hierarchy 
{
    private GameMaster? Master { get; set; }
    internal readonly List<Transform> Objects = new();
    public Hierarchy() { }
    public Hierarchy(GameMaster master)
    {
        Master = master;
    }
    public void AssignMaster(GameMaster master) => Master = master;

    public void UpdateAll() {
        if (Master == null) throw new Exception("Not assigned game master to hierarchy");
        foreach (var trans in Objects) trans.OnUpdate(Master);
    }

    public void DrawAll()
    {
        if (Master == null) throw new Exception("Not assigned game master to hierarchy");

        // draw the hierarchy here with draw order
        foreach (Layer layer in Layer.List)
        {
            foreach (Transform transform in Objects)
            {
                if (transform.Layer != layer) continue;
                transform.OnDraw(Master);
            }
        }
    }
}
