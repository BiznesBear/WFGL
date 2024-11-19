using WFGL.Physics;

namespace WFGL.Shapes;

public struct Edge(Vec2 start, Vec2 end)
{
    public Vec2 Start = start;
    public Vec2 End = end;
}
