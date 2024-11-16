using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;

/// <summary>
/// Renders multiple bitmaps at once. Need to be rendered manually. 
/// </summary>
/// <param name="m"></param>
public class StaticRenderHierarchy(GameMaster m) : Hierarchy(m), IDrawable
{
    private Bitmap? staticRender;
    public Bitmap GetRender() => staticRender ?? throw new ArgumentNullException("Null group render");
    public void Render()
    {
        staticRender = new(GetMaster().RenderSize.Width, GetMaster().RenderSize.Height);
        using var renderer = Graphics.FromImage(staticRender);
        foreach (var obj in GetObjects())
        {
            if (obj is IDrawable idraw)
                idraw.Draw(GetMaster(), renderer);
        }
    }
    protected override void OnEntityDraw(Entity entity) { }
    public void Draw(GameMaster m, Graphics r)
    {
        r.DrawImage(GetRender(), 0, 0);
    }
}