namespace WFGL;

public class Sprite : IAsset<Image>
{
    private Image? source { get; set; } 
    public Vector2 Scale { get; set; } = Vector2.One;
    public string FilePath { get; private set; } = "";
    public Image Source => source ?? throw new Exception("");
    public Sprite() { }
    public Sprite(string path) 
    {
        FilePath = path;

        Load(FilePath);
    }
    public void Load(string filePath)
    {
        source = Image.FromFile(filePath);
    }
}
