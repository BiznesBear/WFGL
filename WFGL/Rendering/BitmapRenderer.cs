using WFGL.Core;
using WFGL.Objects;
using System.Drawing.Imaging.Effects;
using WFGL.Physics;
namespace WFGL.Rendering;

/// <summary>
/// Bitmap rendered directly on renderer.
/// </summary>
public class BitmapRenderer : Transform, IDrawable
{
    public Bitmap Source { get; set; }
    public Effect? Effect { get; set; }
    public Bitmap Bitmap { get=> bmp ?? throw new NullReferenceException("Missing bitmap refrence. Bitmap is not baked"); private set => bmp = value; }

    private Bitmap? bmp;

    public float BitmapRotation { get; set; }
    public Vec2 Pivot { get; set; } = Vec2.Zero;
    public Size RealSize => new((int)(Source.Width * Scale.X), (int)(Source.Height * Scale.Y));

    public BitmapRenderer(Bitmap source, Effect? effect)
    {
        Source = source;
        Effect = effect;
        Bake();
    }
    public BitmapRenderer(Bitmap source) : this(source, null) { }
    public BitmapRenderer(string filePath) : this(new Bitmap(filePath)) { }

    public void Draw(GameMaster m, Graphics r)
    {
        Size size = RealSize.VirtualizePixel(m.MainView);
        Vec2 vize = size.ToVec2(m.VirtualScale);
        Point pos = (Position - (vize * Pivot)).ToPoint(m.VirtualScale);

        if(BitmapRotation != 0)
        {
            r.TranslateTransform(pos.X, pos.Y);
            r.RotateTransform(BitmapRotation);
            r.DrawImage(Bitmap, 0, 0, size.Width, size.Height);
            r.ResetTransform();
        }
        else
        {
            r.DrawImage(Bitmap, pos.X, pos.Y, size.Width, size.Height);
        }
    }

    public Bitmap Bake()
    {
        Bitmap = Source;
        if(Effect is not null)
            Bitmap.ApplyEffect(Effect);
        return Bitmap;
    }
}
