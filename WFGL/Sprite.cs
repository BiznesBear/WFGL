using WFGL.Core;
namespace WFGL;

public class Sprite : Transform
{
    public Image? source;
    public Image Source => source ?? throw new Exception("Cannot load sprite"); 
    public Vector2 RealSize => new(Source.Width * Size.X * SpriteSize, Source.Height * Size.Y * SpriteSize);
    public float SpriteSize { get; set; } = 1f;
    public Sprite() { }
    public Sprite(string filePath)
    {
        source = LoadFromSource(filePath).source;
    }
    public static Sprite LoadFromSource(string filePath) => new() { source = Image.FromFile(filePath) };

    public override void OnDraw(GameMaster master)
    {
        master.DrawSprite(this);
    }
}
