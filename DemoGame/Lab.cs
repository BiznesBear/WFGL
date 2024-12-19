using System.Drawing;
using WFGL.Core;
using WFGL.Input;

namespace DemoGame;


public class LabMaster : GameMaster
{
    private readonly SolidBrush backgroundBrush = new(Color.FromArgb(22, 22, 22));
    
    public LabMaster(GameWindow w) : base(w)
    {
        GameWindow.RegisterInput(new LabInput());
    }

    protected override void OnDraw()
    {
        base.OnDraw();
        Renderer.FillRectangle(backgroundBrush, new(0, 0, VirtualSize.Width, VirtualSize.Height));
        
        DrawBitmap(Assets.player, new Point());
    }
} 
public class LabInput : InputHandler
{
    
}