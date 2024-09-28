namespace WFGL;

public class Sprite : IAsset<Bitmap>
{
    private Bitmap? source { get; set; } 
    public Vector2 Scale { get; set; } = Vector2.One;
    public string FilePath { get; private set; } = "";
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
    public Bitmap GetSource() => source ?? throw new WFGLNullInstanceError("Null image source");
}
