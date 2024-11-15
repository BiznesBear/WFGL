using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;

public abstract class Renderer : Transform, IDrawable
{
    public Hroup? Hroup { get; set; }

    public virtual void Draw(GameMaster m, Graphics r)
    {
        if (Hroup != null) return;
    }
}

public abstract class PenRenderer : Renderer
{
    public Pen Pen { get; set; } = Pens.Yellow;
}