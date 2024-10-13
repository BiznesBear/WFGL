using WFGL.Core;
using WFGL.Input;
using System.Windows.Forms;
using WFGL.ThirdDimension;
using System.Drawing;
namespace DemoGame;



public class RenderTestMaster : GameMaster
{
    private WFGL.Hierarchy objects;
    public ShapeRenderer cube = new();
    public RenderTestMaster(GameWindow w) : base(w)
    {
        RegisterInput(new RenderTestInput(this));

        objects = new(this);
        objects.Objects = [
            cube
            ];
        RegisterHierarchy(objects);
    }
    protected override void OnDraw()
    {
        DrawRectangle(Color.FromArgb(22,22,22),new(0,0,RenderSize.X,RenderSize.Y));
    }
}

public class RenderTestInput : InputHandler
{
    public RenderTestInput(GameMaster g) : base(g)
    {
        
        if (IsKeyPressed(Keys.Left)) { }
        if (IsKeyPressed(Keys.Right)) { }

    }
    protected override void OnKeyDown(Keys key)
    {
        if (key==(Keys.Up)) { Program.renderTestInstance.cube.Rot.X += 10; }
        if (key == (Keys.Down)) { Program.renderTestInstance.cube.Rot.X -= 10; }
    }
    protected override void OnMouseWheel(int delta)
    {
        Program.renderTestInstance.MainCamera.Fov += delta / 6;
    }
}
