using System.Drawing.Drawing2D;
using WFGL.Input;
namespace WFGL.Core;

public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    #region Settings
    public bool IsFullScreen { get; private set; }
    public bool WindowAspectLock { get; set; } = false;
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
    public Point RenderSize => new((int)VirtualScale.FactorX * VirtualUnit.SCALING, (int)VirtualScale.FactorX * VirtualUnit.SCALING);

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

        renderBuffer = new Bitmap(RenderSize.X, RenderSize.Y);
        Renderer = Graphics.FromImage(renderBuffer);

        Time.SetFps(Time.DEFALUT_FPS);

        ResetRenderBuffer();
    }
    private void ResetRenderBuffer()
    {
        renderBuffer.Dispose();
        renderBuffer = new Bitmap(RenderSize.X, RenderSize.Y);

        Renderer = Graphics.FromImage(renderBuffer);
       
        Renderer.SetClip(new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
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
        ResetRenderBuffer() ;
        OnResized();
    }
    public void NormalScreen()
    {
        GameWindow.FormBorderStyle = GameWindow.BorderStyle;
        GameWindow.WindowState = FormWindowState.Normal;
        GameWindow.TopMost = false;
        GameWindow.ClientSize = windowSizeTemp;
        IsFullScreen = false;
        ResetRenderBuffer();
        OnResized();
    }
    private void Resized(object? sender, EventArgs e) 
    {
        if(WindowAspectLock) GameWindow.ClientSize = MainCamera.GetAspect();
        ResetRenderBuffer();
        OnResized();  
    }

    public GameWindow GetWindow() => GameWindow;

    #endregion

    #region Logic
    private void Update(object? sender, EventArgs e)
    {
        GameWindow.Invalidate(new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
        OnUpdate();
        WhenUpdate?.Invoke(this);
    }
    private void Draw(object? sender, PaintEventArgs e) 
    {
        OnDraw();
        WhenDraw?.Invoke(this);

        e.Graphics.SetClip((new Rectangle(0, 0, RenderSize.X, RenderSize.Y)));
        //e.Graphics.DrawImageUnscaled(renderBuffer, new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
        e.Graphics.DrawImageUnscaled(renderBuffer, new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
    }

    #endregion

    #region Drawing

    public readonly Pen defaultPen = new(Color.Red, 1);
    public readonly SolidBrush defaultBrush = new(Color.DarkSlateGray);

    public void DrawLine(Pen pen, Point pos1, Point pos2)
    {
        Renderer.DrawLine(pen, pos1, pos2);
    }
    public void DrawLine(Color color, float width, Point pos1, Point pos2) => DrawLine(new Pen(color, width), pos1, pos2);
    public void DrawLine(Point pos1, Point pos2) => DrawLine(Color.Red, 1, pos1, pos2);


    public void DrawRect(Pen pen, Point position, Point size)
    {
        Renderer.DrawRectangle(pen, new(position, size.PushToSize()));
    }
    public void DrawRect(Color color, float width,Point position, Point size) => DrawRect(new Pen(color, width),position, size);
    public void DrawRect(Point position, Point size) => DrawRect(Color.Red, 1, position, size);


    public void DrawRectangle(Pen pen, Point position, Point size)
    {
        Renderer.DrawRectangle(pen, new(position, size.PushToSize()));
        Renderer.FillRectangle(pen.Brush, new(position, size.PushToSize()));
    }
    public void DrawRectangle(Color color, float width, Point position, Point size)
    {
        var pen = new Pen(color, width);
        DrawRectangle(pen, position, size);
    }
    public void DrawRectangle(Color color, Point position, Point size) => DrawRectangle(color,1, position, size);
    public void DrawRectangle(Point position, Point size) => DrawRectangle(Color.DarkSlateGray,1, position, size);

    public void DrawSprite(Sprite sprite,Point position)
    {
        Point size = new Point(sprite.GetSource().Width, sprite.GetSource().Height).VirtualizePixel(MainCamera);
        Renderer.DrawImage(sprite.GetSource(), position.X, position.Y, size.X, size.Y);
    }
    public void DrawText(UI.StringRenderer text)
    {
        float dynamicFontSize = text.BaseSize * MainCamera.Scaler.FactorX * VirtualUnit.SCALING;
        if (dynamicFontSize < 0.02) return;
        Font dynamicFont = new(text.Font.FontFamily, dynamicFontSize);
        Point pos = text.Position.ToPoint(VirtualScale);
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
        Mouse.Position = e.Location;
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
