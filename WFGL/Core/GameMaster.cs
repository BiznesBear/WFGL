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

    public VirtualUnit VirtualScale => new VirtualUnit(VirtualScaleX, VirtualScaleY).NormalizeByMin();
    protected float VirtualScaleX => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Width);
    protected float VirtualScaleY => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Height);


    // input
    private readonly HashSet<Keys> pressedKeys = new();

    private Graphics Renderer { get; set; }
    public Camera MainCamera { get; private set; }


    private Stopwatch frameStopwatch;
    private int frames;
    public GameMaster(GameWindow window)
    {
        // window
        Console.Title = Application.ProductName??"Game console";
        Application.EnableVisualStyles();
        GameWindow = window;

        // window events
        GameWindow.Paint += Draw;
        GameWindow.Resize += Resized;
        GameWindow.KeyDown += KeyDown;
        GameWindow.KeyUp += KeyUp;
        // time
        TimeMaster.Timer = Timer;
        TimeMaster.Timer.Tick += Update;
        frameStopwatch = new Stopwatch();
        Renderer = GameWindow.CreateGraphics();
        MainCamera = new(this);
        frameStopwatch.Start();
        Time.SetFps(Time.DEFALUT_FPS);

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

        // use somthing like this for rotating objects 
        //Renderer.TranslateTransform(squarePosition.X + squareSize / 2, squarePosition.Y + squareSize / 2);
        //Renderer.RotateTransform(rotationAngle);
        // Renderer.ResetTransform();
    }

    //public void DrawStaticText(string content, StaticText text)
    //{
    //    float dynamicFontSize = text.baseSize; 
    //    Font dynamicFont = new(text.font.FontFamily, dynamicFontSize);

    //    Renderer.DrawString(content, dynamicFont, Brushes.Black, pos.X, pos.Y);
    //    dynamicFont.Dispose();
    //}
    #endregion

    #region Drawing

    public void DrawRect(Pixel position, Pixel size)
    {
        var pen = new Pen(Color.Red, 1);
        Renderer.DrawRectangle(pen,new(position.PushToPoint(),size.PushToSize()));
    }
    public void DrawRectangle(Pixel position, Pixel size)
    {
        var pen = new Pen(Color.Red, 1);
        Renderer.DrawRectangle(pen, new(position.PushToPoint(), size.PushToSize()));
        Renderer.FillRectangle(pen.Brush, new(position.PushToPoint(), size.PushToSize()));
    }
    public void DrawSprite(SpriteRenderer sprite,Pixel size)
    {
        Pixel pixel = sprite.Position.ToPixel(VirtualScale);
        //Renderer.TranslateTransform(squarePosition.X + squareSize / 2, squarePosition.Y + squareSize / 2);
        //Renderer.RotateTransform(rotationAngle);
        // Renderer.ResetTransform();
        Renderer.DrawImage(sprite.Source, pixel.X, pixel.Y, size.X, size.Y);
    }
    #endregion

    #region Input
    private void KeyDown(object? sender, KeyEventArgs e)
    {
        pressedKeys.Add(e.KeyCode);
        OnInput();
    }

    private void KeyUp(object? sender, KeyEventArgs e)
    {
        pressedKeys.Remove(e.KeyCode);
    }
    public bool IsKeyPressed(Keys keys) { return pressedKeys.Contains(keys); }

    #endregion

    #region VirtualFunctions
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
    protected virtual void OnInput() { }


    #endregion
}
