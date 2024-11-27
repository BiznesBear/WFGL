using WFGL.Physics;

namespace WFGL.Core;

// TODO: Add more drawing methods
public abstract partial class GameMaster
{
    #region Line
    public void DrawLine(Pen pen, Vec2 pos1, Vec2 pos2)
        => Renderer.DrawLine(pen, pos1.ToPoint(VirtualScale), pos2.ToPoint(VirtualScale));
    public void DrawLine(Color color, Vec2 pos1, Vec2 pos2) 
    {
        using var p = new Pen(color, 1);
        DrawLine(p,pos1,pos2);
    }
    public void DrawLine(Vec2 pos1, Vec2 pos2) => DrawLine(defaultPen.Color, pos1, pos2);

    #endregion

    #region Box
    public void DrawBox(Pen pen, Box box)
       => Renderer.DrawRectangle(pen, box.GetRectangle(VirtualScale));
    public void DrawBox(Color color, Box box)
    {
        using var p = new Pen(color);
        DrawBox(p, box);
    }
    public void DrawBox(Box box) => DrawBox(defaultPen.Color, box);

    #endregion

    #region FilledBox
    public void DrawFilledBox(Brush brush, Box box) 
        => Renderer.FillRectangle(brush, box.GetRectangle(VirtualScale));

    public void DrawFilledBox(Color color, Box box)
    {
        using var b = new SolidBrush(color);
        DrawFilledBox(b,box);
    }
    public void DrawFilledBox(Box box) => DrawFilledBox(defaultBrush.Color, box);
    #endregion

    #region Other
    public void DrawBitmap(Bitmap bitmap, Point position)
    {
        Size size = bitmap.Size.VirtualizePixel(MainView);
        Renderer.DrawImage(bitmap, position.X, position.Y, size.Width, size.Height);
    }
    #endregion
}
