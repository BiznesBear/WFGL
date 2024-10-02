using WFGL.Core;

namespace WFGL;
public class Group : Transform
{
    protected List<IObject> Members { get; } = new();
    protected GameMaster Master { get; }
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

    public Group(GameMaster master, Hierarchy hierarchy)
    {
        Master = master;
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
        finRender = new(Master.RenderSize.X, Master.RenderSize.Y);
        renderer = Graphics.FromImage(finRender);
       
        foreach(var obj in Layer.GetObjectsFrom(Layer.SortObjectList(Members)))
        {
            if (obj is IDrawable idraw)
            {
                idraw.Draw(Master, renderer);
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