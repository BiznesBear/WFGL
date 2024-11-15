using WFGL.Core;
using WFGL.Rendering;
namespace WFGL.Objects;

/// <summary>
/// Group and hierarchy in one.
/// </summary>
public abstract class Hroup : Hierarchy
{
    public Hroup(GameMaster m) : base(m) { }
}

/// <summary>
/// Renders multiple bitmaps at once. Need to be rendered manually. 
/// </summary>
/// <param name="m"></param>
public class StaticRenderHroup(GameMaster m) : Hroup(m), IDrawable
{
    private Bitmap? finRender;
    private Graphics? renderer;
    public Hroup? Hroup { get; set; }

    public Bitmap GetRender() => finRender ?? throw new ArgumentNullException("Null group render");
    public void Render()
    {
        finRender = new(GetMaster().RenderSize.Width, GetMaster().RenderSize.Height);
        renderer = Graphics.FromImage(finRender);

        foreach (var obj in GetObjects())
        {
            if (obj is IDrawable idraw)
                idraw.Draw(GetMaster(), renderer);
        }
    }
    public override void OnDraw(GameMaster m)
    {
        Draw(m, m.Renderer);
    }
    public void Draw(GameMaster m, Graphics r)
    {
        r.DrawImage(GetRender(), 0, 0);
    }
}