using WFGL.Pseudo.Physics;

namespace WFGL.Core;

public sealed class Drawer(GameMaster m)
{
    private readonly GameMaster m = m;

    #region Line
    public void DrawLine(Pen pen, Vec2 pos1, Vec2 pos2)
        => m.Renderer.DrawLine(pen, pos1.ToPoint(m.VirtualScale), pos2.ToPoint(m.VirtualScale));
    public void DrawLine(Color color, Vec2 pos1, Vec2 pos2) 
    {
        using var p = new Pen(color, 1);
        DrawLine(p,pos1,pos2);
    }
    public void DrawLine(Vec2 pos1, Vec2 pos2) => DrawLine(m.defaultPen.Color, pos1, pos2);

    #endregion

    #region Box
    public void DrawBox(Pen pen, Box box)
       => m.Renderer.DrawRectangle(pen, box.GetRectangle(m.VirtualScale));
    public void DrawBox(Color color, Box box)
    {
        using var p = new Pen(color);
        DrawBox(p, box);
    }
    public void DrawBox(Box box) => DrawBox(m.defaultPen.Color, box);

    #endregion

    #region FilledBox
    public void DrawFilledBox(Brush brush, Box box) 
        => m.Renderer.FillRectangle(brush, box.GetRectangle(m.VirtualScale));

    public void DrawFilledBox(Color color, Box box)
    {
        using var b = new SolidBrush(color);
        DrawFilledBox(b,box);
    }
    public void DrawFilledBox(Box box) => DrawFilledBox(m.defaultBrush.Color, box);
    #endregion

    #region Other
    public void DrawBitmap(Bitmap bitmap, Vec2 position, Vec2 scale)
    {
        Size size = new Size((int)(bitmap.Size.Width * scale.X), (int)(bitmap.Size.Height * scale.Y)).VirtualizePixel(m.MainView);
        Point pos = position.ToPoint(m.VirtualScale);

        m.Renderer.DrawImage(bitmap, pos.X, pos.Y, size.Width, size.Height);
    }
    public void DrawBitmap(Bitmap bitmap, Vec2 position) => DrawBitmap(bitmap, position, Vec2.One);

    #endregion
}
