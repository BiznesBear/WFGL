using WFGL;
using WFGL.Core;
using WFGL.Input;
using WFGL.Fun;
using WFGL.ThirdDimension;

using System.Drawing;
using System.Windows.Forms;

namespace DemoGame;


public class RenderTestMaster : GameMaster
{
    private Hierarchy objects;
    public RotatingCube cube = new();
    public RenderTestMaster(GameWindow w) : base(w)
    {
        RegisterInput(new RenderTestInput(this));

        objects = new(this);
        objects.Objects = 
        [
            cube
        ];
        RegisterHierarchy(objects);
    }
    protected override void OnUpdate()
    {
        var input = InputMaster;
        float speed = 9f;
        Vec2 direction = Vec2.Zero;
        if (input.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.W)) direction += new Vec2(0f, speed);
        if (input.IsKeyPressed(Keys.S)) direction -= new Vec2(0f, speed);
        cube.Position += direction.Normalize() * speed * TimeMaster.DeltaTime;


        if (input.IsKeyPressed(Keys.Left)) MainCamera.Position -= new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Right)) MainCamera.Position += new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Up)) MainCamera.Position -= new Vec2(0f, 0.07f);
        if (input.IsKeyPressed(Keys.Down)) MainCamera.Position += new Vec2(0f, 0.07f);
        ResetRenderClip();
    }
    protected override void OnDraw()
    {
        // background
        DrawRectangle(Color.FromArgb(22,22,22),new(0,0,RenderSize.X,RenderSize.Y));
    }
}

public class RenderTestInput : InputHandler
{
    public RenderTestInput(GameMaster g) : base(g) { }
    protected override void OnMouseWheel(int delta)
    {
        Program.renderTestInstance.MainCamera.Fov += delta / 4f;
    }
}
