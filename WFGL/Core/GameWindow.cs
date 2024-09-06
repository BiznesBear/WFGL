namespace WFGL.Core;

public class GameWindow : Form
{
    public GameWindow()
    {
        DoubleBuffered = true;
        MaximizeBox = false;
    }
    public void SetWindowOptions(GameWindowOptions options)
    {
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Text = options.Title;
        Size = options.Size;
        BackColor = options.Background;
        Icon = options.Icon;
    }
}

public struct GameWindowOptions
{
    public string Title {  get; set; }
    public Size Size {  get; set; }
    public Color Background {  get; set; }
    public Icon Icon { get; set; } 

    public readonly static GameWindowOptions Default = new()
    {
        Title = "WFGL game window",
        Size = new(900, 600),
        Background = Color.Black
    };
}