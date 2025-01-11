namespace WFGL.Pseudo.Shapes;
public class Cube : ShapeRenderer
{
    public Cube() : base(new Shape(Vec3.One, new int[,] {
            { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, // Back edges
            { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 }, // Front edges
            { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 } })) // Connects 
    { }

    public override void UpdateRot()
    {
        Shape.Vertices =
        [
            new(-RealMesh.X, -RealMesh.Y, -RealMesh.Z), // Left bottom back
            new(RealMesh.X, -RealMesh.Y, -RealMesh.Z),  // Right bottom back
            new(RealMesh.X, RealMesh.Y, -RealMesh.Z),   // Right top back
            new(-RealMesh.X, RealMesh.Y, -RealMesh.Z),  // Left top back
            new(-RealMesh.X, -RealMesh.Y, RealMesh.Z),  // Left bottom front
            new(RealMesh.X, -RealMesh.Y, RealMesh.Z),   // Right bottom front
            new(RealMesh.X, RealMesh.Y, RealMesh.Z),    // Right top front
            new(-RealMesh.X, RealMesh.Y, RealMesh.Z)    // Left top front
        ];
        base.UpdateRot();
    }
}