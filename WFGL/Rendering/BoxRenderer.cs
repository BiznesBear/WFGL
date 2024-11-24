using WFGL.Core;
using WFGL.Physics;
using WFGL.Objects;

namespace WFGL.Rendering;

public abstract class BoxRendererBase : Transform
{
    public Box Box { get; set; } = new Box();
}

public class BoxRenderer : BoxRendererBase, IPenDrawable
{
    public Pen Pen { get; set; } = new(Color.Red,2);

    public void Draw(GameMaster m, Graphics r)
    {
        m.DrawBox(Pen, Box);
    }
}

public class FilledBoxRenderer : BoxRendererBase, IBrushDrawable
{
    public Brush Brush { get; set; } = Brushes.Green;
    public void Draw(GameMaster m, Graphics r)
    {
        m.DrawFilledBox(Brush, Box);
    }
}
