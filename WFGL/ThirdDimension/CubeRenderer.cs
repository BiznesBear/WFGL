using System.Numerics;
using WFGL.Core;
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


public class SpriteRenderer3D : Transform, IDrawable
{
    public Hroup? Hroup { get; set; }
    public Vector2 SpriteSize { get; set; } = new Vector2(1, 1); 
    public Vector3 Rot = new(0.7f, 0.7f, 0.7f); 
    public Vector3 Pos3D => new(Position.X, Position.Y, 0); 
    private Image texture;

    public SpriteRenderer3D()
    {
        texture = Image.FromFile("mozg-masz-wypadlo-ci.jpg");
    }

    private static PointF Project(Vector3 point, float viewWidth, float viewHeight, float fov, float viewDistance)
    {
        // Adjust projection for perspective
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


        Vector3[] vertices = new Vector3[]
        {
                new Vector3(-SpriteSize.X / 2, -SpriteSize.Y / 2, 0), // Bottom-left
                new Vector3(SpriteSize.X / 2, -SpriteSize.Y / 2, 0),  // Bottom-right
                new Vector3(SpriteSize.X / 2, SpriteSize.Y / 2, 0),   // Top-right
                new Vector3(-SpriteSize.X / 2, SpriteSize.Y / 2, 0),  // Top-left
        };

        // Apply 3D rotation (around its center) to the vertices
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rot.Y, Rot.X, Rot.Z);
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Transform(vertices[i], rotationMatrix) + Pos3D;
        }

        // Project 3D vertices to 2D screen space
        PointF[] projectedPoints = new PointF[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            projectedPoints[i] = Project(vertices[i], viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);
        }

        PointF[] textureCoords = new PointF[]
        {
                new PointF(0, 0),                // Top-left corner of the texture
                new PointF(texture.Width, 0),    // Top-right corner
                new PointF(texture.Width, texture.Height), // Bottom-right corner
                new PointF(0, texture.Height)    // Bottom-left corner
        };

        // Draw the sprite using a texture brush
        using (TextureBrush brush = new TextureBrush(texture))
        {
            // Set up the texture brush transformation for correct mapping
            brush.Transform = new System.Drawing.Drawing2D.Matrix(
                (projectedPoints[1].X - projectedPoints[0].X) / texture.Width,
                (projectedPoints[1].Y - projectedPoints[0].Y) / texture.Height,
                (projectedPoints[3].X - projectedPoints[0].X) / texture.Width,
                (projectedPoints[3].Y - projectedPoints[0].Y) / texture.Height,
                projectedPoints[0].X,
                projectedPoints[0].Y
            );

            // Fill the polygon (flat quad) with the sprite texture
            r.FillPolygon(brush, projectedPoints);
        }
    }
}

public class Test3d : Transform, IDrawable
{
    public Hroup? Hroup { get; set; }
    public Vector3 Cube = new(1, 1, 1);
    public Vector3 Rot = new(0.7f, 0.7f, 0.7f);
    public Vector3 Pos3D => new(Position.X, Position.Y, 0);
    public Vector3[] Vertices { get; set; } = new Vector3[8];
    public Pen pen = new(Color.Blue, 3);
    private Image texture;

    public Test3d()
    {
        // Load the texture (replace with the actual path)
        texture = Image.FromFile("mozg-masz-wypadlo-ci.jpg");
    }

    private static PointF Project(Vector3 point, float viewWidth, float viewHeight, float fov, float viewDistance)
    {
        // Adjust the projection to account for perspective
        float factor = fov / (viewDistance + point.Z);
        float x = point.X * factor + viewWidth / 2;
        float y = -point.Y * factor + viewHeight / 2;
        return new PointF(x, y);
    }

    private static Vector3 CalculateNormal(Vector3[] vertices, int[] face)
    {
        // Calculate normal using cross product
        Vector3 a = vertices[face[1]] - vertices[face[0]];
        Vector3 b = vertices[face[2]] - vertices[face[0]];
        return Vector3.Cross(a, b);
    }

    private static bool IsBackFace(Vector3 normal, Vector3 cameraDirection)
    {
        return Vector3.Dot(normal, cameraDirection) < 0;
    }
    public void Draw(GameMaster m, Graphics r)
    {
        float viewWidth = m.RenderSize.X;
        float viewHeight = m.RenderSize.Y;

        // Transform the vertices of the cube
        Vertices = new Vector3[]
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
        for (int i = 0; i < Vertices.Length; i++)
            Vertices[i] = Vector3.Transform(Vertices[i], rotationMatrix) + Pos3D;

        // Camera Direction for back-face culling
        Vector3 cameraDirection = new Vector3(0, 0, -1); // Assuming the camera looks in the negative Z Direction

        // Define the faces of the cube (each face is defined by 4 vertices)
        int[][] faces = new int[][]
        {
                new int[] { 0, 1, 2, 3 }, // Back face
                new int[] { 4, 5, 6, 7 }, // Front face
                new int[] { 0, 1, 5, 4 }, // Bottom face
                new int[] { 2, 3, 7, 6 }, // Top face
                new int[] { 0, 3, 7, 4 }, // Left face
                new int[] { 1, 2, 6, 5 }  // Right face
        };

        // Draw each face, apply back-face culling
        foreach (var face in faces)
        {
            // Calculate the normal of the face for back-face culling
            Vector3 normal = CalculateNormal(Vertices, face);
            if (IsBackFace(normal, cameraDirection))
            {
                // Skip drawing this face if it's a back-face
                continue;
            }

            DrawTexturedFace(m, r, face);
        }
    }

    // Function to draw textured faces
    private void DrawTexturedFace(GameMaster m, Graphics r, int[] faceIndexes)
    {
        float viewWidth = m.RenderSize.X;
        float viewHeight = m.RenderSize.Y;

        // Get the four vertices of the face
        PointF[] face = new PointF[4];
        for (int i = 0; i < faceIndexes.Length; i++)
        {
            Vector3 point = Vertices[faceIndexes[i]];
            face[i] = Project(point, viewWidth, viewHeight, m.MainCamera.Fov, m.MainCamera.ViewDistance);
        }

        // Corresponding texture coordinates (UV mapping)
        PointF[] textureCoords = new PointF[]
        {
                new PointF(0, 0), // Top-left corner of the texture
                new PointF(texture.Width, 0), // Top-right corner of the texture
                new PointF(texture.Width, texture.Height), // Bottom-right corner
                new PointF(0, texture.Height) // Bottom-left corner
        };

        // Draw the textured face
        using (TextureBrush brush = new TextureBrush(texture))
        {
            // Set up the texture brush transformation for correct mapping
            brush.Transform = new System.Drawing.Drawing2D.Matrix(
                (face[1].X - face[0].X) / texture.Width,
                (face[1].Y - face[0].Y) / texture.Height,
                (face[3].X - face[0].X) / texture.Width,
                (face[3].Y - face[0].Y) / texture.Height,
                face[0].X,
                face[0].Y
            );

            r.FillPolygon(brush, face);
        }
    }
}