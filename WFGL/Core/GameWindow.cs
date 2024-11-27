using WFGL.Input;
using WFGL.Utilities;

namespace WFGL.Core;

public class GameWindow : Form
{
    public bool InputHandlerIsNull => inputHandler is null;
    public FormBorderStyle borderStyle = FormBorderStyle.Sizable;

    private InputHandler? inputHandler;

    public GameWindow(GameWindowOptions options)
    {
        DoubleBuffered = true;
        SetWindowOptions(options);
    }
    public GameWindow() : this(GameWindowOptions.Default) { }
    public void SetWindowOptions(GameWindowOptions options)
    {
        SetStyle(ControlStyles.EnableNotifyMessage, options.EnableNotifyMessage);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, options.OptimizedDoubleBuffer);
        SetStyle(ControlStyles.UserPaint, options.UserPaint);
        SetStyle(ControlStyles.AllPaintingInWmPaint, options.AllPaintingInWmPaint);

        MaximizeBox = options.MaximizeButton;
        FormBorderStyle = borderStyle;
        Text = options.Title;
        ClientSize = options.Size;
        MinimumSize = options.MinSize;
        BackColor = options.Background;
        Icon = options.Icon;
    }

    public void RegisterInput(InputHandler handler)
    {
        if (inputHandler != null) DeregisterInput(inputHandler);
        Wrint.Register(handler.GetType().Name);

        inputHandler = handler;

        KeyDown += handler.KeyDown;
        KeyUp += handler.KeyUp;

        MouseMove += handler.MouseMoved;
        MouseDown += handler.MouseDown;
        MouseUp += handler.MouseUp;
        MouseDoubleClick += handler.MouseDoubleClicked;
        MouseEnter += handler.MouseEnter;
        MouseLeave += handler.MouseLeave;
        MouseWheel += handler.MouseWheel;
    }

    public void DeregisterInput(InputHandler handler)
    {
        Wrint.Deregister(handler.GetType().Name);
        inputHandler = null;
        KeyDown -= handler.KeyDown;
        KeyUp -= handler.KeyUp;
        MouseMove -= handler.MouseMoved;
        MouseDown -= handler.MouseDown;
        MouseUp -= handler.MouseUp;
        MouseDoubleClick -= handler.MouseDoubleClicked;
        MouseEnter -= handler.MouseEnter;
        MouseLeave -= handler.MouseLeave;
        MouseWheel -= handler.MouseWheel;
    }

    public InputHandler GetInput() => inputHandler ?? throw new NullReferenceException("No input handler assigned");
}

public struct GameWindowOptions
{
    public string Title {  get; set; }
    public Size Size {  get; set; }
    public Size MinSize {  get; set; }
    public Color Background {  get; set; }
    public Icon Icon { get; set; } 
    public bool MaximizeButton { get; set; }  
    public bool EnableNotifyMessage { get; set; } 
    public bool OptimizedDoubleBuffer { get; set; }  
    public bool UserPaint { get; set; }  
    public bool AllPaintingInWmPaint { get; set; }  

    public readonly static GameWindowOptions Default = new()
    {
        Title = "WFGL game window",
        Size = new(700, 700),
        MinSize = new(250, 250),
        EnableNotifyMessage = true,
        OptimizedDoubleBuffer = true,
        UserPaint = true,
        AllPaintingInWmPaint = true,
    };
}
