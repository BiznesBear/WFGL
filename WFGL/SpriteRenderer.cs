using WFGL.Core;
namespace WFGL;

public class SpriteRenderer : Transform
{
    public Image Source => Sprite.GetSource() ?? throw new Exception("Cannot load sprite");

    public Sprite Sprite { get; set; } = new();

    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Pixel RealSize => new((int)(Source.Width * Size.X * Sprite.Scale.X) , (int)(Source.Height * Size.Y * Sprite.Scale.Y));
    public SpriteRenderer() { }
    public SpriteRenderer(string filePath)
    {
        Sprite = new(filePath);
    }

    public override void OnDraw(GameMaster m)
    {
        m.DrawSprite(this,RealSize.VirtualizePixel(m.MainCamera));
    }
}
