using WFGL.Core;
using WFGL.Objects;
using WFGL.Shapes;
namespace WFGL.Components;

public class TestingCube : Cube
{
    public float rotatingSpeed = 0.01f;

    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        Pen = Pens.Red;
    }

    public override void OnUpdate(GameMaster m)
    {
        base.OnUpdate(m);
        Rot += rotatingSpeed;
    }
}
