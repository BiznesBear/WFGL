using WFGL.Core;
using WFGL.Objects;
using WFGL.Physics;

namespace WFGL.Rendering;

public class ShapeRenderer : Transform, IPenDrawable
{
    public List<Vec2> Vertices = new();
    public Pen Pen { get; set; } = Pens.Black;

    public ShapeRenderer(params Vec2[] verticles)
    {
        Vertices = [.. verticles];
    }


    public void Draw(GameMaster m, Graphics r)
    {
        r.DrawPolygon(Pen,GetPoints().ToArray());
    }

    public IEnumerable<Point> GetPoints()
    {
        foreach (var v in Vertices)
            yield return v.ToPoint(GetMaster().VirtualScale);
    }
}
