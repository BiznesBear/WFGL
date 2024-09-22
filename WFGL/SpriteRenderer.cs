using WFGL.Core;
namespace WFGL;

public class SpriteRenderer : Transform, IDrawable
{
    public Image Source => Sprite.Source;
    public Sprite Sprite { get; set; } = new();

    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Pixel RealSize => new((int)(Source.Width * Scale.X * Sprite.Scale.X) , (int)(Source.Height * Scale.Y * Sprite.Scale.Y));


    public SpriteRenderer() { }
    public SpriteRenderer(Sprite sprite)
    {
        Sprite = sprite;
    }
    public SpriteRenderer(string filePath) : this(new Sprite(filePath)) { }
    
    public override void OnDraw(GameMaster m)
    {
        var r = m.Renderer;
        Pixel pixel = Position.ToPixel(m.VirtualScale);
        Pixel size = RealSize.VirtualizePixel(m.MainCamera);
        r.DrawImage(Source, pixel.X, pixel.Y, size.X, size.Y);
    }
}
