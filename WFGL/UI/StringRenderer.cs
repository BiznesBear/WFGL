namespace WFGL.UI;

public class StringRenderer : Transform
{
    public float BaseSize { get; private set; } = 12;
    public Font Font { get; private set; }
    public string Content { get; set; } = "";

    public Color Color { get; set; } = Color.Black;

    public StringRenderer(Font f) 
    {
        Font = f;
        BaseSize = f.Size;
    }
    public StringRenderer(Font f, string text) : this(f)
    {
        Content = text;
    }
    public StringRenderer(Font f,string text, Color color) : this(f,text) 
    {
        Color = color;
    }

    public override void OnDraw(Core.GameMaster m)
    {
        m.DrawText(this);
    }
    public void UpdateFont(Font f) 
    {
        Font = f;
        BaseSize = f.Size;
    }
}