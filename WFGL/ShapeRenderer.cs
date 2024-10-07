namespace WFGL;

public class ShapeRenderer : Transform, IDrawable
{
    public Vector2[] points = [];
    public ShapeRenderer(params Vector2[] ps)
    {
        points = ps;
    }

    public Hroup? Hroup { get; set; }

    public void Draw(Core.GameMaster m, Graphics r)
    {
        r.DrawLines(m.defaultPen, GetPoints().ToArray());
    }
    private IEnumerable<Point> GetPoints()
    {
        foreach (var p in points)
            yield return p.ToPoint(GetMaster().VirtualScale);
    }
    public override void OnDraw(Core.GameMaster m)
    {
        if (Hroup != null) return; 
        Draw(m,m.Renderer);
    }
}
