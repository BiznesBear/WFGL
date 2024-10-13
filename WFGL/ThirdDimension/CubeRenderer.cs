using System.Numerics;
using WFGL.Core;

namespace WFGL.ThirdDimension;

public class ShapeRenderer : Transform,IDrawable
{
    public Hroup? Hroup { get; set; }
    public Vector3 Cube = new(1, 1, 1); // Wymiary kostki
    public Vector3 Rot = new(0.7f, 1f, 0.7f); // Obrót kostki
    public Vector3 Pos => new(Position.ToPoint(GetMaster()).X, Position.ToPoint(GetMaster()).Y, 0); // Pozycja kostki
    public virtual Vector3[] Verticles { get; set; } = [];
    public Pen pen = new(Color.Blue, 3);
    private Bitmap? rend;
    private Graphics? render;
    private PointF Project(Vector3 point, float viewWidth, float viewHeight, float fov, float viewDistance)
    {
        float factor = fov / (viewDistance + point.Z);
        float x = point.X * factor + viewWidth / 2;
        float y = -point.Y * factor + viewHeight / 2; 
        return new PointF(x, y);
    }
    public override void OnUpdate(GameMaster m)
    {
        Rot.X += 0.01f;
        Rot.Y += 0.01f;
        Rot.Z += 0.01f;
    }

    public void Draw(GameMaster m, Graphics r)
    {
        
        float viewWidth = m.RenderSize.X;
        float viewHeight = m.RenderSize.Y;

        Vector3[] vertices = 
        {
            new(-Cube.X, -Cube.Y, -Cube.Z), // Left bottom back
            new(Cube.X, -Cube.Y, -Cube.Z),  // Right bottom back
            new(Cube.X, Cube.Y, -Cube.Z),   // Right top back
            new(-Cube.X, Cube.Y, -Cube.Z),  // Left top back
            new(-Cube.X, -Cube.Y, Cube.Z),  // Left bottom front
            new(Cube.X, -Cube.Y, Cube.Z),   // Right bottom front
            new(Cube.X, Cube.Y, Cube.Z),    // Right top front
            new(-Cube.X, Cube.Y, Cube.Z)    // Left top front
        };

        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rot.Y, Rot.X, Rot.Z);
        for (int i = 0; i < vertices.Length; i++)
        { 
            vertices[i] = Vector3.Transform(vertices[i], rotationMatrix) + Pos;
        }

        // Verticles indexes
        int[,] edges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, // Back edges
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 }, // Front edges
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }  // Connects
        };
        rend = new(m.RenderSize.X, m.RenderSize.Y);
        render = Graphics.FromImage(rend);
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            Vector3 point1 = vertices[edges[i, 0]];
            Vector3 point2 = vertices[edges[i, 1]];

            PointF p1 = Project(point1, viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);
            PointF p2 = Project(point2, viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);

            render.DrawLine(pen, p1, p2);
        }
        Point pixel = Position.ToPoint(m);
        Point size = rend.Size.PushToPoint().VirtualizePixel(m.MainCamera);

        r.DrawImage(rend, pixel.X, pixel.Y, size.X * VirtualUnit.SCALING, size.Y * VirtualUnit.SCALING);
    }
}
