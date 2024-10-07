using WFGL.Core;
namespace WFGL;

public class Group<T> : HashSet<T>
{
    public HashSet<Hierarchy> Hierarchys { get; } = new();

    public Group(params Hierarchy[] hierarchys)
    { 
        foreach(Hierarchy hierarchy in hierarchys) 
            Hierarchys.Add(hierarchy);
        Update();
    }

    public void Update()
    {
        Clear();
        foreach(var hierarchy in Hierarchys)
        {
            foreach(IObject obj in hierarchy.Objects)
            {
                if (obj is not T t) continue;
                Add(t);
            }
        }
    }
}

/// <summary>
/// This is hierarchy, but also group.
/// </summary>
public abstract class Hroup : Hierarchy
{
    public Hroup(GameMaster m) : base(m) { }
}

/// <summary>
/// Need to be rendered manually. Perfect for drawing backgrounds and static objects.
/// </summary>
/// <param name="m"></param>
public class StaticRenderHroup(GameMaster m) : Hroup(m), IDrawable
{
    private Bitmap? finRender;
    private Graphics? renderer;
    public Hroup? Hroup { get; set; }

    public Bitmap GetRender() => finRender ?? throw new WFGLNullInstanceError("Null group render");
    public void Render()
    {
        finRender = new(GetMaster().RenderSize.X, GetMaster().RenderSize.Y);
        renderer = Graphics.FromImage(finRender);

        foreach (var obj in LayerMaster.GetObjectsFrom(GetMaster().LayerMaster, LayerMaster.SortObjectList(Objects)))
        {
            if (obj is IDrawable idraw)
            {
                idraw.Draw(GetMaster(), renderer);
            }
        }
    }
    public override void OnDraw(GameMaster m)
    {
        Draw(m, m.Renderer);
    }
    public void Draw(GameMaster m, Graphics r)
    {
        r.DrawImage(GetRender(), Position.X, Position.Y);
    }
}