using System.Numerics;
using WFGL.Core;
using WFGL.Objects;
using WFGL.Physics;
using WFGL.Rendering;

namespace WFGL.Shapes;

public class ShapeRenderer : Transform3D, IPenDrawable
{
    public Pen Pen { get; set; } = Pens.Black;
    public Shape Shape { get; set; }
    public Vec3 RealMesh => Shape.Mesh * Scale;

    public ShapeRenderer(Shape shape)
    {
        Shape = shape;
        UpdateRot();
    }

    public void Draw(GameMaster m, Graphics r)
    {
        for (int i = 0; i < Shape.Edges.GetLength(0); i++)
        {
            Vec3 point1 = Shape.Vertices[Shape.Edges[i, 0]];
            Vec3 point2 = Shape.Vertices[Shape.Edges[i, 1]];

            m.Renderer.DrawLine(Pen, point1.ToPoint(m.VirtualScale), point2.ToPoint(m.VirtualScale));
        }
    }

    public IEnumerable<Point> GetPoints()
    {
        foreach (var v in Shape.Vertices)
            yield return (v + Position).ToPoint(GetMaster().VirtualScale);
    }

    public virtual void UpdateRot()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rot.Y, Rot.X, Rot.Z);
        for (int i = 0; i < Shape.Vertices.Length; i++)
            Shape.Vertices[i] = Vector3.Transform(Shape.Vertices[i].ToVector3(), rotationMatrix).ToVec3() + Position;
    }
}
