using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;

public class StaticRenderer : Transform, IDrawable
{
    public List<IDrawable> drawables = new();
    public bool needsRedraw;

    private Bitmap? staticBitmap;


    public Bitmap GetRender() => staticBitmap ?? throw new ArgumentNullException("Null render");
    public virtual void Render()
    {
        staticBitmap = new(GetMaster().RenderSize.Width, GetMaster().RenderSize.Height);

        using (Graphics g = Graphics.FromImage(staticBitmap))
        {
            foreach(IDrawable drawable in drawables)
                drawable.Draw(GetMaster(), g);
        }
        needsRedraw = false;
    }

    public void Draw(GameMaster m, Graphics r)
    {
        if (needsRedraw || staticBitmap == null)
        {
            Render();
            return;
        }
        Point pos = Position.ToPoint(m.VirtualScale);
        m.Renderer.DrawImage(staticBitmap, pos.X, pos.Y);
    }
}
