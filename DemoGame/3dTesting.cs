using WFGL.Core;
using WFGL.Input;

using System.Drawing;
using System.Windows.Forms;
using WFGL.Objects;
using WFGL.Physics;
using WFGL.Pseudo.ThirdDimension;

namespace DemoGame;


public class TreDeTestMaster : GameMaster
{
    private Hierarchy objects;
    public RotatingCube cube = new();
    public TreDeTestMaster(GameWindow w) : base(w)
    {
        RegisterInput(new TreDeTestInput(this));

        objects = new(this);
        objects.Objects = 
        [
            cube
        ];
        RegisterHierarchy(objects);
    }
    protected override void OnUpdate()
    {
        float speed = 9f;
        Vec2 direction = Vec2.Zero;
        if (InputMaster.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (InputMaster.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);
        if (InputMaster.IsKeyPressed(Keys.W)) direction += new Vec2(0f, speed);
        if (InputMaster.IsKeyPressed(Keys.S)) direction -= new Vec2(0f, speed);
        cube.Position += direction.Normalize() * speed * TimeMaster.DeltaTime;


        if (InputMaster.IsKeyPressed(Keys.Left)) MainView.Position -= new Vec2(0.07f, 0f);
        if (InputMaster.IsKeyPressed(Keys.Right)) MainView.Position += new Vec2(0.07f, 0f);
        if (InputMaster.IsKeyPressed(Keys.Up)) MainView.Position -= new Vec2(0f, 0.07f);
        if (InputMaster.IsKeyPressed(Keys.Down)) MainView.Position += new Vec2(0f, 0.07f);
        ResetRenderClip();
    }
    protected override void OnDraw()
    {
        // background
        DrawRectangle(Color.FromArgb(22,22,22),new(0,0, RenderSize.Width, RenderSize.Height));
    }
}

public class TreDeTestInput : InputHandler
{
    public TreDeTestInput(GameMaster g) : base(g) { }
    protected override void OnMouseWheel(int delta)
    {
        Program.renderTestInstance.MainView.Fov += delta / 4f;
    }
}
