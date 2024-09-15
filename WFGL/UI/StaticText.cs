namespace WFGL.UI;

public class StaticText : Transform
{
    public float BaseSize { get; private set; } = 12;
    public Font Font { get; private set; }
    public string Content { get; set; }
    public Canvas Canvas { get; }
    public Brush Brush { get; set; } = Brushes.Black;

    public StaticText(Canvas parent, Font f, string text)
    {
        Font = f;
        BaseSize = f.Size;
        Canvas = parent;
        Content = text;
    }
    public StaticText(Canvas parent, Font f, Brush brush,string text)
    {
        Font = f;
        BaseSize = f.Size;
        Canvas = parent;
        Content = text;
        Brush = brush;
    }
    public override void OnDraw(Core.GameMaster m)
    {
        m.DrawStaticText(this);
    }
    public void UpdateFont(Font f) 
    {
        Font = f;
        BaseSize = f.Size;
    }
}