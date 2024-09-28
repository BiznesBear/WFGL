using System.Drawing.Drawing2D;
using WFGL.Input;
namespace WFGL.Core;

public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    #region Settings
    public bool IsFullScreen { get; private set; }
    public bool WindowAspectRatioLock { get; set; } = false;
    public SmoothingMode Smoothing { get; set; } = SmoothingMode.None;
    public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

    #endregion

    #region RenderingAndTime
    public Camera MainCamera { get; private set; }
    public Time TimeMaster { get; } = new();
    public Graphics Renderer { get; private set; }

    private InputHandler? inputHandler;
    public InputHandler InputMaster => inputHandler ?? throw new WFGLNullInstanceError("Cannot use unregistered input handler");
    #endregion

    // events 
    public event GameMasterEventArgs? WhenUpdate;
    public event GameMasterEventArgs? WhenDraw;

    // other
    public Bitmap renderBuffer;
    private Size windowSizeTemp;

    #region Getters
    public VirtualUnit VirtualScale => new VirtualUnit(RealVirtualScaleX, RealVirtualScaleY).Normalize();
    public float RealVirtualScaleX => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Width);
    public float RealVirtualScaleY => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Height);

    public Point WindowCenter => new(GameWindow.ClientSize.Width / 2, GameWindow.ClientSize.Height / 2);
    public Size WindowSize => new(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);
    public Size RenderSize => new((int)VirtualScale.FactorX * VirtualUnit.SCALING, (int)VirtualScale.FactorX * VirtualUnit.SCALING);

    #endregion

    public GameMaster(GameWindow window)
    {
        // window
        GameWindow = window;
        windowSizeTemp = GameWindow.ClientSize;

        // window events
        GameWindow.Paint += Draw;
        GameWindow.ResizeEnd += Resized;

        GameWindow.MouseMove += MouseMoved;
        float aspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
        CameraOptions options = new(new Size((int)aspectRatio,(int)aspectRatio),GameWindow.ClientSize);
        MainCamera = new(this, options);


        TimeMaster.Timer.Tick += Update;

        renderBuffer = new Bitmap(RenderSize.Width, RenderSize.Height);
        Renderer = Graphics.FromImage(renderBuffer);

        Time.SetFps(Time.DEFALUT_FPS);

        ResetRenderBuffer();

    }
    private void ResetRenderBuffer()
    {
        renderBuffer.Dispose();
        renderBuffer = new Bitmap(RenderSize.Width, RenderSize.Height);

        Renderer = Graphics.FromImage(renderBuffer);
        Renderer.SetClip(new Rectangle(0, 0, RenderSize.Width, RenderSize.Height));
        Renderer.SmoothingMode = Smoothing;
        Renderer.InterpolationMode = Interpolation;
        OnRenderBufferReset();
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
    private void Resized(object? sender, EventArgs e) 
    {
        if(WindowAspectRatioLock) GameWindow.ClientSize = MainCamera.GetAspect();
        ResetRenderBuffer();
        OnResized();  
    }

    public GameWindow GetWindow() => GameWindow;

    #endregion

    #region Logic
    private void Update(object? sender, EventArgs e)
    {
        GameWindow.Invalidate(new Rectangle(0, 0, RenderSize.Width, RenderSize.Height));
        OnUpdate();
        WhenUpdate?.Invoke(this);
    }
    private void Draw(object? sender, PaintEventArgs e) 
    {
        OnDraw();
        WhenDraw?.Invoke(this);

        e.Graphics.SetClip((new Rectangle(0, 0, RenderSize.Width, RenderSize.Height)));
        e.Graphics.DrawImageUnscaled(renderBuffer, new Rectangle(0, 0, RenderSize.Width, RenderSize.Height));
    }

    #endregion

    #region Drawing

    private readonly Pen defaultPen = new(Color.Red, 1);
    private readonly SolidBrush defaultBrush = new(Color.DarkSlateGray);

    public void DrawRect(Color color,float width, Point position, Size size)
    {
        Renderer.DrawRectangle(defaultPen, new(position, size));
    }
    public void DrawRect(Color color, Point position, Size size) => DrawRect(color, 1, position, size);
    public void DrawRect(Point position, Size size) => DrawRect(Color.Red, 1, position, size);

    public void DrawRectangle(Color color, float width, Point position, Size size)
    {
        var pen = new Pen(color, width);
        Renderer.DrawRectangle(pen, new(position, size));
        Renderer.FillRectangle(pen.Brush, new(position, size));
    }
    public void DrawRectangle(Color color, Point position, Size size) => DrawRectangle(color,1, position, size);
    public void DrawRectangle(Point position, Size size) => DrawRectangle(Color.DarkSlateGray,1, position, size);

    public void DrawSprite(Sprite sprite,Point position)
    {
        Pixel size = new Pixel(sprite.GetSource().Width, sprite.GetSource().Height).VirtualizePixel(MainCamera);
        Renderer.DrawImage(sprite.GetSource(), position.X, position.Y, size.X, size.Y);
    }
    public void DrawSprite(Sprite sprite, Pixel position) => DrawSprite(sprite, position.PushToPoint());
    public void DrawText(UI.StringRenderer text)
    {
        float dynamicFontSize = text.BaseSize * MainCamera.Scaler.FactorX * VirtualUnit.SCALING;
        if (dynamicFontSize < 0.02) return;
        Font dynamicFont = new(text.Font.FontFamily, dynamicFontSize);
        Pixel pos = text.Position.ToPixel(VirtualScale);
        Brush brush = new SolidBrush(text.Color);
        Renderer.DrawString(text.Content, dynamicFont, brush, pos.X, pos.Y);
        brush.Dispose();
        dynamicFont.Dispose();
    }
    #endregion

    #region Input
    public void RegisterInput(InputHandler handler)
    {
        if(inputHandler != null) RemoveInput(inputHandler);
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

    #region EventFunctions

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
    protected virtual void OnResized() { }

    /// <summary>
    /// Called when render buffer is reseted.
    /// </summary>
    protected virtual void OnRenderBufferReset() { }
    #endregion
}
