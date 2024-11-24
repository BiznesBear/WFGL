using System.Drawing;
using WFGL.Components;
using WFGL.Core;
using WFGL.Input;
using WFGL.Objects;
using WFGL.Physics;
using WFGL.Rendering;
namespace DemoGame;


public class LabMaster : GameMaster
{
    public LabMaster(GameWindow w) : base(w)
    {
        GameWindow.RegisterInput(new LabInput());
    }

    private SolidBrush backgroundBrush = new(Color.FromArgb(22, 22, 22));
    protected override void OnDraw()
    {
        base.OnDraw();
        Renderer.FillRectangle(backgroundBrush, new(0, 0, VirtualSize.Width, VirtualSize.Height));
        
        DrawBitmap(Assets.player,new Point());
    }
} 
public class LabInput : InputHandler
{
    
}