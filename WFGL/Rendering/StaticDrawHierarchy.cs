using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;

/// <summary>
/// Optimalization method for hierarchy. Renders objects only when Render method is called.
/// </summary>
/// <param name="m"></param>
public class StaticDrawHierarchy(GameMaster m) : Hierarchy(m), IDrawable
{
    private Bitmap? staticRender;
    public Bitmap GetRender() => staticRender ?? throw new NullReferenceException("Null group render");
    public void Render()
    {
        staticRender = new(Master.VirtualSize.Width, Master.VirtualSize.Height);
        using var renderer = Graphics.FromImage(staticRender);
        foreach (var obj in GetObjects())
            if (obj is IDrawable idraw)
                idraw.Draw(Master, renderer);
    }
    protected override void OnEntityDraw(Entity entity) { } // just do nothing
    public void Draw(GameMaster m, Graphics r) => r.DrawImage(GetRender(), 0, 0);
}