using WFGL.Core;
namespace WFGL;

public class SpriteRenderer : Transform, IDrawable
{
    public Image Source => Sprite.GetSource();
    public Sprite Sprite { get; set; } = new();
    public Group? Group { get; set; }
    // TODO: Rework how real size of any object is get
    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Point RealSize => new((int)(Source.Width * Scale.X * Sprite.Scale.X) , (int)(Source.Height * Scale.Y * Sprite.Scale.Y));


    public Vector2 GetRealVirtualSize(GameMaster m) //TODO: GetRealVirtualSize
    {
        Vector2 sourceSize = Converts.ToVector2(Source.Size,(m.VirtualScale));
        Vector2 size = new Vector2((int)(sourceSize.X * Scale.X * Sprite.Scale.X), (int)(sourceSize.Y * Scale.Y * Sprite.Scale.Y));
        return Vector2.Zero;
    }
    public SpriteRenderer() { }
    public SpriteRenderer(Sprite sprite)
    {
        Sprite = sprite;
    }
    public SpriteRenderer(string filePath) : this(new Sprite(filePath)) { }
    public override void OnDraw(GameMaster m)
    {
        if (Group != null) return;
        Draw(m,m.Renderer);
    }

    public void Draw(GameMaster m,Graphics r)
    {
        Point pixel = Position.ToPoint(m.VirtualScale);
        Point size = RealSize.VirtualizePixel(m.MainCamera);
        r.DrawImage(Source, pixel.X, pixel.Y, size.X, size.Y);
    }
}
public class CollidingSprite : SpriteRenderer, ICollide
{
    public Vector2 ColliderSize => RealSize.VirtualizePixel(Master.MainCamera).ToVector2(Master.VirtualScale);
    public Vector2 ColliderPosition => Position;

    private GameMaster? master;
    private GameMaster Master => master ?? throw new WFGLNullInstanceError("Null master instance in colliding sprite");

    public CollidingSprite() : base() { }
    public CollidingSprite(string filePath) : base(filePath) { }
    public CollidingSprite(Sprite sprite) : base(sprite) { }

    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        master = m;
    }
}