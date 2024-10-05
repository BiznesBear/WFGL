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
        float dynamicFontSize = BaseSize * m.MainCamera.Scaler * VirtualUnit.SCALING;
        if (dynamicFontSize < 0.02) return;
        Font dynamicFont = new(Font.FontFamily, dynamicFontSize);
        Point pos = Position.ToPoint(m.VirtualScale);
        Brush brush = new SolidBrush(Color);
        m.Renderer.DrawString(Content, dynamicFont, brush, pos.X, pos.Y);
        brush.Dispose();
        dynamicFont.Dispose();
    }
    public void UpdateFont(Font f) 
    {
        Font = f;
        BaseSize = f.Size;
    }
}