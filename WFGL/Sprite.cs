namespace WFGL;

public class Sprite : IAsset<Bitmap>
{
    private Bitmap? source { get; set; } 
    public Vec2 Scale { get; set; } = Vec2.One;
    // TODO: add more properties 

    
    public string? FilePath { get; private set; } = null;
    public Sprite() { }
    public Sprite(string path) 
    {
        FilePath = path;
        Load(FilePath);
    }
    public Sprite(Bitmap image)
    {
        source = image;
    }
    public void Load(string filePath)
    {
        source = new(filePath);
    }
    public Bitmap GetSource() => source ?? throw new ArgumentNullException("Null bitmap source");
}
