using WFGL.Core;
using WFGL.Objects;

namespace WFGL.Rendering;

/// <summary>
/// Bitmap rendered directly on renderer.
/// </summary>
public class BitmapRenderer : Renderer
{
    public Bitmap Source { get; set; }
    public float BitmapRotation { get; set; }

    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Point RealSize => new((int)(Source.Width * Scale.X), (int)(Source.Height * Scale.Y));

    public BitmapRenderer(Bitmap bitmap)
    {
        Source = bitmap;       
    }
    public BitmapRenderer(string filePath) : this(new Bitmap(filePath)) { }

    public override void Draw(GameMaster m, Graphics r)
    {
        base.Draw(m, r);

        Point size = RealSize.VirtualizePixel(m.MainView);
        Point pos = Position.ToPoint(m.VirtualScale);

        r.TranslateTransform(pos.X, pos.Y);
        r.RotateTransform(BitmapRotation);
        r.DrawImage(Source, 0, 0, size.X, size.Y);
        r.ResetTransform();
    }
}
