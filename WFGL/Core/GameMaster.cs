using WFGL.Input;
namespace WFGL.Core;

public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    public bool IsFullScreen { get; private set; }
    

    public VirtualUnit VirtualScale => new VirtualUnit(VirtualScaleX, VirtualScaleY).Normalize();
    protected float VirtualScaleX => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Width);
    protected float VirtualScaleY => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Height);

    public Pixel WindowCenter => new(GameWindow.ClientSize.Width/2, GameWindow.ClientSize.Height/2);
    public Pixel WindowSize => new(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);

    // input

    public Camera MainCamera { get; private set; }

    public Time TimeMaster { get; } = new();
    private Graphics Renderer { get; set; }

    private InputHandler? inputHandler;
    public InputHandler InputMaster => inputHandler ?? throw new Exception("Cannot use unregistered input handler");

    private Size windowSizeTemp;


    public GameMaster(GameWindow window)
    {
        // window
        Console.Title = Application.ProductName ?? "WFGL game";

        GameWindow = window;

        // window events
        GameWindow.Paint += Draw;
        GameWindow.Resize += Resized;

        GameWindow.MouseMove += MouseMoved;
        

        // time

        TimeMaster.Timer.Tick += Update;

        Renderer = GameWindow.CreateGraphics();

        MainCamera = new(this);

        Time.SetFps(Time.DEFALUT_FPS);
    }


    public VirtualUnit GetVUnitRatio()
    {
        // do tego wystarczy już tylko znormalizować ten kwadrat
        VirtualUnit unit = new()
        {
            FactorX = VirtualScaleX - VirtualScaleX % MainCamera.AspectRatio.Width,
            FactorY = VirtualScaleY - VirtualScaleY % MainCamera.AspectRatio.Height
        };
        return unit;
    }

    #region Window
    public void Load()
    {
        Time.Start();
        OnLoad();
        GameWindow.ShowDialog();
    }

    public void FullScreen() 
    {
        windowSizeTemp = GameWindow.ClientSize;
        GameWindow.FormBorderStyle = FormBorderStyle.None;
        GameWindow.WindowState = FormWindowState.Maximized; 
        GameWindow.TopMost = true;
        IsFullScreen = true;
    }
    public void NormalScreen()
    {
        GameWindow.FormBorderStyle = GameWindow.BorderStyle;
        GameWindow.WindowState = FormWindowState.Normal;
        GameWindow.TopMost = false;
        GameWindow.ClientSize = windowSizeTemp;
        IsFullScreen = false;
    }
    private void Resized(object? sender, EventArgs e) { OnResize();  }

    public GameWindow GetWindow() => GameWindow;
    public Graphics GetRenderer() => Renderer;

    #endregion

    #region Logic
    private void Update(object? sender, EventArgs e)
    {
        GameWindow.Invalidate();
        OnUpdate();
    }
    private void Draw(object? sender, PaintEventArgs e) 
    {
        Renderer = e.Graphics;
        OnDraw();
    }


    #endregion

    #region Drawing
    public void DrawRect(Color color,float width, Pixel position, Pixel size)
    {
        var pen = new Pen(color, width);
        Renderer.DrawRectangle(pen, new(position.PushToPoint(), size.PushToSize()));
    }
    public void DrawRect(Color color,Pixel position, Pixel size) => DrawRect(color, 1, position, size);
    public void DrawRect(Pixel position, Pixel size) => DrawRect(Color.Red, 1, position, size);



    public void DrawRectangle(Color color, float width, Pixel position, Pixel size)
    {
        var pen = new Pen(color, width);
        Renderer.DrawRectangle(pen, new(position.PushToPoint(), size.PushToSize()));
        Renderer.FillRectangle(pen.Brush, new(position.PushToPoint(), size.PushToSize()));
    }
    public void DrawRectangle(Color color,Pixel position, Pixel size) => DrawRectangle(color,1, position, size);
    public void DrawRectangle(Pixel position, Pixel size) => DrawRectangle(Color.DarkSlateGray,1, position, size);

    public void DrawSprite(Sprite sprite,Pixel position)
    {
        Pixel size = new Pixel(sprite.Source.Width, sprite.Source.Height).VirtualizePixel(MainCamera);
        Renderer.DrawImage(sprite.Source, position.X, position.Y, size.X, size.Y);
    }

    public void DrawStaticText(UI.StaticText text)
    {
        float dynamicFontSize = text.BaseSize * MainCamera.Scaler.FactorX * VirtualUnit.SCALING;
        Font dynamicFont = new(text.Font.FontFamily, dynamicFontSize);
        Pixel pos = text.Position.ToPixel(VirtualScale);
        Renderer.DrawString(text.Content, dynamicFont, text.Brush, pos.X, pos.Y);
        dynamicFont.Dispose();
    }

    public void DrawPseudoDarkness(int alpha=140)
    {
        DrawRectangle(Color.FromArgb(alpha, 0, 0, 0),Pixel.Zero,WindowSize); 
    }
    #endregion

    #region Input

    public void RegisterInput(InputHandler handler)
    {
        if(inputHandler != null)
        {
            RemoveInput(inputHandler);
        }

        inputHandler = handler;
        GameWindow.KeyDown += handler.KeyDown;
        GameWindow.KeyUp += handler.KeyUp;
    }
    public void RemoveInput(InputHandler handler)
    {
        inputHandler = null;
        GameWindow.KeyDown -= handler.KeyDown;
        GameWindow.KeyUp -= handler.KeyUp;
    }

    private void MouseMoved(object? sender, MouseEventArgs e)
    {
        Mouse.Position = e.Location.PushToPixel();
    }

    #endregion

    #region OtherFunctions

    /// <summary>
    /// When game is first loaded. Load every thing here.
    /// </summary>
    protected virtual void OnLoad() { }

    /// <summary>
    /// Called every frame. Here update game logic.
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// Called when the game window is drawed and always before update.
    /// </summary>
    protected virtual void OnDraw() { }

    /// <summary>
    /// Called when windows is resized.
    /// </summary>
    protected virtual void OnResize() { }
    #endregion
}
