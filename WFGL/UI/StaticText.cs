using WFGL.Core;
namespace WFGL.UI;


public class StaticText : Transform
{
    public float baseSize;
    public Font font;
    public string content;
    public Canvas canvas;
    public Brush brush = Brushes.Black;

    public StaticText(Canvas parent, Font f, string text)
    {
        font = f;
        canvas = parent;
        content = text;
    }
    public override void OnDraw(GameMaster m)
    {
        Pixel pos = Position.ToPixel(m.VirtualScale);
        m.GetRenderer().DrawString(content, font, Brushes.Black, pos.X, pos.Y); //TODO: Rework this (now this is not scaling with virtualization :< )
    }
    public void UpdateText(string text) => content = text;
    public void UpdateFont(Font f) => font = f;
}