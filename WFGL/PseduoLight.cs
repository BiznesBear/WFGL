using WFGL.Core;

namespace WFGL;

public class PseduoLight : Transform
{
    public float intensity=1;

    public float radius=160;

    public Color color = Color.GhostWhite;
    public byte alpha = 6;

    private Bitmap? bitmap;
    private bool needsRedraw = true;

    private void GenerateLightBitmap(GameMaster m)
    {
        int size = (int)(radius * 2);
        bitmap = new Bitmap(size, size);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            Pixel center = new((int)radius, (int)radius);
            for (int r = (int)radius; r > 0; r -= 4)
            {
                int a = (int)(alpha * (float)(r / radius));
                using SolidBrush brush = new(Color.FromArgb(a, color.R, color.G, color.B));
                g.FillEllipse(brush, center.X - r, center.Y - r, r * 2, r * 2);
            }
        }
        needsRedraw = false;
    }
    public override void OnDraw(GameMaster m)
    {
        if (needsRedraw || bitmap == null) 
        {
            GenerateLightBitmap(m);
            return;
        }
        Pixel pos = Position.ToPixel(m.VirtualScale);
        m.GetRenderer().DrawImage(bitmap, pos.X - (int)radius, pos.Y - (int)radius); 
    }
}
