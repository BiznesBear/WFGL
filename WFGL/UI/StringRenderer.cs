namespace WFGL.UI;

public class StringRenderer : Transform
{
    public float BaseSize { get; private set; } = 12;
    public Font Font { get; private set; }
    public string Content { get; set; }
    public Canvas Canvas { get; }

    public Color Color { get; set; } = Color.Black;

    public StringRenderer(Canvas parent, Font f, string text)
    {
        Font = f;
        BaseSize = f.Size;
        Canvas = parent;
        Content = text;
    }
    public StringRenderer(Canvas parent, Font f, Color color,string text)
    {
        Font = f;
        BaseSize = f.Size;
        Canvas = parent;
        Content = text;
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