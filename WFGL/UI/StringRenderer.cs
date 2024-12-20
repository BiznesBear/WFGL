using WFGL.Core;
using WFGL.Objects;
using WFGL.Rendering;

namespace WFGL.UI;

public class StringRenderer : Transform, IDrawable
{
    public const string DEFALUT_FONT_NAME = "Consolas";
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
    public void UpdateFont(Font f) 
    {
        Font = f;
        BaseSize = f.Size;
    }

    public void Draw(GameMaster m, Graphics r)
    {
        float dynamicFontSize = BaseSize * m.MainView.Scaler * VirtualUnit.SCALING;
        if (dynamicFontSize < 0.02) return;

        using Font dynamicFont = new(Font.FontFamily, dynamicFontSize);
        using Brush brush = new SolidBrush(Color);
        Point pos = Position.ToPoint(m.VirtualScale);
        
        m.Renderer.DrawString(Content, dynamicFont, brush, pos.X, pos.Y);
        brush.Dispose();
    }
}