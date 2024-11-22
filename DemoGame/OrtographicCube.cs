using System.Drawing;
using System.Windows.Forms;

using WFGL.Core;
using WFGL.Input;
using WFGL.Objects;
using WFGL.Physics;
using WFGL.Components;

namespace DemoGame;


public class OrtographicCubeMaster : GameMaster
{
    private Hierarchy objects;
    public TestingCube cube = new();
    public OrtographicCubeMaster(GameWindow w) : base(w)
    {
        GameWindow.RegisterInput(new OrtographicCubeInput());

        objects = new(this);
        objects.Objects = 
        [
            cube
        ];
        RegisterHierarchy(objects);
    }


    const float rotSp = 0.06f;
    protected override void OnUpdate()
    {
        float speed = 3f;

        Vec2 direction = Vec2.Zero;
        if (InputMaster.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (InputMaster.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);
        if (InputMaster.IsKeyPressed(Keys.W)) direction -= new Vec2(0f, speed);
        if (InputMaster.IsKeyPressed(Keys.S)) direction += new Vec2(0f, speed);
        cube.Position += (Vec3)(direction.Normalize() * speed * TimeMaster.DeltaTime);


        if (InputMaster.IsKeyPressed(Keys.Down)) cube.Rot += new Vec3(rotSp, 0, rotSp); 
        if (InputMaster.IsKeyPressed(Keys.Left)) cube.Rot += new Vec3(0, rotSp, 0);
        if (InputMaster.IsKeyPressed(Keys.Right)) cube.Rot += new Vec3(0, 0, rotSp);
        if (InputMaster.IsKeyPressed(Keys.Up)) cube.Rot += new Vec3(rotSp, 0, 0);

        cube.UpdateRot();

    }
    private SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(22, 22, 22));
    protected override void OnDraw()
    {
        // background
        Renderer.FillRectangle(backgroundBrush, new(0,0, VirtualSize.Width, VirtualSize.Height));
    }
}

public class OrtographicCubeInput : InputHandler
{
    protected override void OnMouseDown(MouseButtons buttons)
    {
        Program.renderTestInstance.cube.Scale += buttons==MouseButtons.Left? 0.1f : -0.1f;
        base.OnMouseDown(buttons);
    }
}