using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    public bool IsFullScreen { get; private set; }
    private Size windowSizeTemp;

    private Time TimeMaster { get; } = new();
    private Timer Timer { get; } = new();

    public VirtualUnit VirtualScale => new VirtualUnit(VirtualScaleX, VirtualScaleY).Normalize();
    protected float VirtualScaleX => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Width);
    protected float VirtualScaleY => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Height);


    public VirtualUnit GetVUnitRatio()
    {
        // do tego wystarczy już tylko znormalizować ten kwadrat

        VirtualUnit unit = new()
        {
            FactorX = VirtualScaleX - VirtualScaleX % MainCamera.AspectRatio.Width,
            FactorY = VirtualScaleY - VirtualScaleY % MainCamera.AspectRatio.Height
        };
        //VirtualUnit unit = new()
        //{
        //    FactorX =  0,
        //    FactorY = 0
        //};
        return unit;
    }
    public Pixel WindowCenter => new(GameWindow.ClientSize.Width/2, GameWindow.ClientSize.Height/2);
    // input
    private readonly HashSet<Keys> pressedKeys = new();

    private Graphics Renderer { get; set; }
    public Camera MainCamera { get; private set; }

    // fps counter 
    private Stopwatch frameStopwatch;
    private int frames;
    public GameMaster(GameWindow window)
    {
        // window
        Console.Title = Application.ProductName ?? "Game console";
        GameWindow = window;

        // window events
        GameWindow.Paint += Draw;
        GameWindow.Resize += Resized;
        GameWindow.KeyDown += KeyDown;
        GameWindow.KeyUp += KeyUp;

        // time
        TimeMaster.Timer = Timer;
        TimeMaster.Timer.Tick += Update;

        Renderer = GameWindow.CreateGraphics();

        MainCamera = new(this);

        Time.SetFps(Time.DEFALUT_FPS);

        frameStopwatch = new Stopwatch();
        frameStopwatch.Start();
    }
    #region Window
    public void CreateWindow()
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
        // update all game lib stuff here

        // counting current fps
        frames++;
        if (frameStopwatch.ElapsedMilliseconds >= 1000)
        {
            TimeMaster.framesPerSecond = frames / (frameStopwatch.ElapsedMilliseconds / 1000f);
            frames = 0;
            frameStopwatch.Restart();
        }
        // calculating delta time
        DateTime currentTime = DateTime.Now;
        TimeMaster.deltaTime = (currentTime - TimeMaster.previousTime).TotalSeconds;
        TimeMaster.previousTime = currentTime;

        // refreshing and updating other stuff
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
        var pen = new Pen(Color.DarkSlateGray, 1);
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
        using SolidBrush darkBrush = new(Color.FromArgb(alpha, 0, 0, 0)); 
        Renderer.FillRectangle(darkBrush, 0, 0, GameWindow.Width, GameWindow.Height); 
    }
    #endregion

    #region Input
    private void KeyDown(object? sender, KeyEventArgs e)
    {
        pressedKeys.Add(e.KeyCode);
        OnKeyDown(e.KeyCode);
    }

    private void KeyUp(object? sender, KeyEventArgs e)
    {
        pressedKeys.Remove(e.KeyCode);
        OnKeyUp(e.KeyCode);
    }
    public bool IsKeyPressed(Keys keys) { return pressedKeys.Contains(keys); }

    #endregion

    #region OtherFunctions
    /// <summary>
    /// When game is first loaded. Here create start logic.
    /// </summary>
    protected virtual void OnLoad() { }

    /// <summary>
    /// Called every frame. Here update game logic.
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// Called every time the game window is drawed and always before update. Here draw transforms. 
    /// </summary>
    protected virtual void OnDraw() { }

    /// <summary>
    /// Called every time when window is resized.
    /// </summary>
    protected virtual void OnResize() { }
    /// <summary>
    /// Called every time when any key is pressed down.
    /// </summary>
    protected virtual void OnKeyDown(Keys keys) { }
    protected virtual void OnKeyUp(Keys keys) { }

    #endregion
}
