using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;



/// <summary>
/// Currently not aviable 
/// </summary>
public class StaticRenderer : Transform, IDrawable
{
    public bool needsRedraw;


    private Size size = new(100,100);
    private Bitmap? staticBitmap;
    public Bitmap GetRender() => staticBitmap ?? throw new ArgumentNullException("Null render");

    public virtual void Render()
    {
        staticBitmap = new(GetMaster().RenderSize.Width, GetMaster().RenderSize.Height);

        staticBitmap = new(size.Width,size.Height);
        using (Graphics g = Graphics.FromImage(staticBitmap))
        {
            // draw everything here 
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
