using WFGL.Core;
namespace WFGL;


// Remeber group is not hierarchy. It's external IObject linking method. But still this needs rework.
public class Group : Transform
{
    protected List<IObject> Members { get; } = new();
    protected Hierarchy Hierarchy { get; }
    public List<IObject> Objects
    {
        get => Members;
        set
        {
            Members.AddRange(value);
            foreach (var obj in value)
            {
                obj.Create(Hierarchy);
                if (obj is IDrawable idraw)
                    idraw.Group = this;
            }
        }
    }

    public Group(GameMaster m, Hierarchy hierarchy)
    {
        master = m;
        Hierarchy = hierarchy;
    }
    public Group(GameMaster master, Hierarchy hierarchy, params IObject[] objs) : this(master, hierarchy)
    {
        Objects = objs.ToList();
    }
}
public class DrawableGroup : Group, IDrawable
{
    private Bitmap? finRender;
    private Graphics? renderer;
    public Group? Group { get; set; }
    public DrawableGroup(GameMaster master, Hierarchy hierarchy) : base(master, hierarchy) { }
    public DrawableGroup(GameMaster master, Hierarchy hierarchy ,params IObject[] objs) : base(master, hierarchy, objs) { }
    
    public Bitmap GetRender() => finRender ?? throw new WFGLNullInstanceError("Null group render");
    public void Render()
    {
        finRender = new(GetMaster().RenderSize.X, GetMaster().RenderSize.Y);
        renderer = Graphics.FromImage(finRender);
       
        foreach(var obj in LayerMaster.GetObjectsFrom(GetMaster().LayerMaster,LayerMaster.SortObjectList(Members)))
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
