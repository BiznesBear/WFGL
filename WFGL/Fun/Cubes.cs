using WFGL.ThirdDimension;

namespace WFGL.Fun;

public class RotatingCube : CubeRenderer
{
    public float Offset { get; set; } = 0.01f;
    public override void OnUpdate(Core.GameMaster m)
    {
        Rot.X += Offset;
        Rot.Y += Offset;
        Rot.Z += Offset;
    }
}