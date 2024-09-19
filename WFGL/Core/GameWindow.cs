namespace WFGL.Core;

public class GameWindow : Form
{
    public FormBorderStyle BorderStyle { get; set; } = FormBorderStyle.Sizable;
    public GameWindow(GameWindowOptions options)
    {
        DoubleBuffered = true;
        MaximizeBox = false;
        SetWindowOptions(options);
    }
    public void SetWindowOptions(GameWindowOptions options)
    {
        FormBorderStyle = BorderStyle;
        Text = options.Title;
        ClientSize = options.Size;
        MinimumSize = options.MinSize;
        BackColor = options.Background;
        Icon = options.Icon;
    }
}

public struct GameWindowOptions
{
    public string Title {  get; set; }
    public Size Size {  get; set; }
    public Size MinSize {  get; set; }
    public Color Background {  get; set; }
    public Icon Icon { get; set; } 

    public readonly static GameWindowOptions Default = new()
    {
        Title = "WFGL game window",
        Size = new(1280, 720),
        MinSize = new(100, 100),
        Background = Color.Black
    };
}