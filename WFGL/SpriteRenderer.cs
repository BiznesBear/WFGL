using WFGL.Core;
namespace WFGL;

public class SpriteRenderer : Transform, IDrawable
{
    public Bitmap Source => Sprite.GetSource();
    public Sprite Sprite { get; set; } = new();
    public Hroup? Hroup { get; set; }

    // TODO: Rework how real size of any object is get
    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Point RealSize => new((int)(Source.Width * Scale.X * Sprite.Scale.X) , (int)(Source.Height * Scale.Y * Sprite.Scale.Y));


    public SpriteRenderer() { }
    public SpriteRenderer(Sprite sprite)
    {
        Sprite = sprite;
    }
    public SpriteRenderer(string filePath) : this(new Sprite(filePath)) { }
    public override void OnDraw(GameMaster m)
    {
        //Draw(m,m.Renderer);
    }

    public void Draw(GameMaster m,Graphics r)
    {
        if (Hroup != null) return;
        Point pixel = Position.ToPoint(m);
        Point size = RealSize.VirtualizePixel(m.MainCamera);

        r.DrawImage(Source, pixel.X, pixel.Y, size.X, size.Y);
    }
}
public class CollidingSprite : SpriteRenderer, Physics.ICollide
{
    public Vec2 ColliderSize => RealSize.VirtualizePixel(GetMaster().MainCamera).ToVector2(GetMaster());
    public Vec2 ColliderPosition => Position;

    public CollidingSprite() : base() { }
    public CollidingSprite(string filePath) : base(filePath) { }
    public CollidingSprite(Sprite sprite) : base(sprite) { }
}