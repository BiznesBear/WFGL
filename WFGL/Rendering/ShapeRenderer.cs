using WFGL.Core;
using WFGL.Physics;

namespace WFGL.Rendering;

public class ShapeRenderer : PenRenderer
{
    public List<Vec2> Vertices = new();
    public ShapeRenderer(params Vec2[] verticles)
    {
        Vertices = [.. verticles];
    }

    public override void Draw(GameMaster m, Graphics r)
    {
        base.Draw(m, r);
        r.DrawPolygon(Pen,GetPoints().ToArray());
    }

    public IEnumerable<Point> GetPoints()
    {
        foreach (var v in Vertices)
            yield return v.ToPoint(GetMaster().VirtualScale);
    }
}
