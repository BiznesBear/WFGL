using System.Numerics;
using WFGL.Core;
using WFGL.Objects;
using WFGL.Rendering;
namespace WFGL.ThirdDimension;

// THIS IS STILL DANGER ZONE WITH EXPERIMENTAL FUNCTIONS
public class CubeRenderer : Transform, IDrawable
{
    public Hroup? Hroup { get; set; }
    public Vector3 Cube = new(1, 1, 1);
    public Vector3 Rot = new(0.7f, 0.7f, 0.7f); 
    public Vector3 Pos3D => new(Position.X,Position.Y,0); 
    public Vector3[] Verticles { get; set; } = [];
    public Pen pen = new(Color.Blue, 3);


    private static PointF Project(Vector3 point, float viewWidth, float viewHeight, float fov, float viewDistance)
    {
        float factor = fov / (viewDistance + point.Z);
        float x = point.X * factor + viewWidth / 2;
        float y = -point.Y * factor + viewHeight / 2; 
        return new PointF(x, y);
    }
    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);

    }
    public void Draw(GameMaster m, Graphics r)
    {
        float viewWidth = m.RenderSize.X;
        float viewHeight = m.RenderSize.Y;

        Verticles =
        [
            new(-Cube.X, -Cube.Y, -Cube.Z), // Left bottom back
            new(Cube.X, -Cube.Y, -Cube.Z),  // Right bottom back
            new(Cube.X, Cube.Y, -Cube.Z),   // Right top back
            new(-Cube.X, Cube.Y, -Cube.Z),  // Left top back
            new(-Cube.X, -Cube.Y, Cube.Z),  // Left bottom front
            new(Cube.X, -Cube.Y, Cube.Z),   // Right bottom front
            new(Cube.X, Cube.Y, Cube.Z),    // Right top front
            new(-Cube.X, Cube.Y, Cube.Z)    // Left top front
        ];

        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rot.Y, Rot.X, Rot.Z);
        for (int i = 0; i < Verticles.Length; i++)
            Verticles[i] = Vector3.Transform(Verticles[i], rotationMatrix) + Pos3D;


        // Verticles indexes
        int[,] edges = new int[,]
        {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, // Back edges
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 }, // Front edges
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }  // Connects
        };

        for (int i = 0; i < edges.GetLength(0); i++)
        {
            Vector3 point1 = Verticles[edges[i, 0]];
            Vector3 point2 = Verticles[edges[i, 1]];

            PointF p1 = Project(point1, viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);
            PointF p2 = Project(point2, viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);
            using var p = new Pen(pen.Color, m.MainCamera.Scaler * pen.Width);
            m.Renderer.DrawLine(p, p1.VirtualizePixel(m.MainCamera), p2.VirtualizePixel(m.MainCamera));
        }
    }
}

public class RotatingCube : CubeRenderer
{
    public float Offset { get; set; } = 0.01f;
    public override void OnUpdate(GameMaster m)
    {
        Rot.X += Offset;
        Rot.Y += Offset;
        Rot.Z += Offset;
    }
}