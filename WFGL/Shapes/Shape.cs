using WFGL.Physics;

namespace WFGL.Shapes;

public class Shape(Vec3 mesh, int[,] edges)
{
    public Vec3 Mesh = mesh;
    public int[,] Edges = edges;
    public Vec3[] Vertices = [];
}
