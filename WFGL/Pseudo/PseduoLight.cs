using WFGL.Core;
using WFGL.Objects;
using WFGL.Rendering;

namespace WFGL.Pseudo;

public class PseduoLight : Transform, IDrawable
{
    public float intensity = 1;

    public float radius = 160;

    public Color color = Color.GhostWhite;
    public byte alpha = 6;

    private Bitmap? bitmap;
    private bool needsRedraw = true;

    public Hroup? Hroup { get; set; }

    private void GenerateLightBitmap(GameMaster m)
    {
        int size = (int)(radius * 2);
        bitmap = new Bitmap(size, size);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            Point center = new((int)radius, (int)radius);
            for (int r = (int)radius; r > 0; r -= 4)
            {
                int a = (int)(alpha * (float)(r / radius));
                using SolidBrush brush = new(Color.FromArgb(a, color.R, color.G, color.B));
                g.FillEllipse(brush, center.X - r, center.Y - r, r * 2, r * 2);
            }
        }
        needsRedraw = false;
    }

    public void Draw(GameMaster m, Graphics r)
    {
        if (needsRedraw || bitmap == null)
        {
            GenerateLightBitmap(m);
            return;
        }
        Point pos = Position.ToPoint(m);

        m.Renderer.DrawImage(bitmap, pos.X - (int)radius, pos.Y - (int)radius);
    }
}
