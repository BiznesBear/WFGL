namespace WFGL;

public class Sprite : IAsset<Image>
{
    private Image? Source { get; set; } 
    public Vector2 Scale { get; set; } = Vector2.One;
    public string FilePath { get; private set; } = "";

    public Sprite() { }
    public Sprite(string path) 
    {
        FilePath = path;
        Load(FilePath);
    }
    public Image GetSource() => Source ?? throw new Exception("Cannot load sprite");
    public void Load(string filePath)
    {
        Source = Image.FromFile(filePath);
    }
}
