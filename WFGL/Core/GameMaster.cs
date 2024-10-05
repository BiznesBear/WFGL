using System.Drawing.Drawing2D;
using WFGL.Input;
namespace WFGL.Core;

public class GameMaster
{

    #region Settings
    public bool IsFullScreen { get; private set; }
    public bool WindowAspectLock { get; set; } = false;
    public SmoothingMode Smoothing { get; set; } = SmoothingMode.None;
    public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

    #endregion

    #region Masters

    private GameWindow GameWindow { get; set; }
    public Camera MainCamera { get; set; }
    public Time TimeMaster { get; } = new();
    public LayerMaster LayerMaster { get; } = new();
    public Graphics Renderer { get; private set; }

    private InputHandler? inputHandler;
    public InputHandler InputMaster => inputHandler ?? throw new WFGLNullInstanceError("Cannot use unregistered input handler");

    #endregion

    // events 
    public event GameMasterEventHandler? WhenUpdate;
    public event GameMasterEventHandler? WhenDraw;

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

    #region Game 

    public Hierarchy MainHierarchy { get; }

    #endregion 

    public GameMaster(GameWindow window)
    {
        // window
        GameWindow = window;
        windowSizeTemp = GameWindow.ClientSize;

        // window events
        GameWindow.Paint += Draw;
        GameWindow.ResizeEnd += Resized;

        

        float aspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
        CameraOptions options = new(new Size((int)aspectRatio,(int)aspectRatio),GameWindow.ClientSize);
        MainCamera = new(this, options);


        TimeMaster.Timer.Tick += Update;

        renderBuffer = new Bitmap(RenderSize.X, RenderSize.Y);
        Renderer = Graphics.FromImage(renderBuffer);

        TimeMaster.SetFps();

        ResetRenderBuffer();

        MainHierarchy = new Hierarchy(this);
        WhenUpdate += MainHierarchy.OnUpdate;
        WhenDraw += MainHierarchy.OnDraw;
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

    public void Load()
    {
        TimeMaster.Start();
        OnLoad();
        GameWindow.ShowDialog();
    }

    #region Window

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

    #region MainHierarchy
    public void RegisterHierarchy(Hierarchy hierarchy)
    {
        MainHierarchy.Register(hierarchy);
    }
    public void UnregisterHierarchy(Hierarchy hierarchy)
    {
        MainHierarchy.Unregister(hierarchy);
    }
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
        OnAfterDraw();

        e.Graphics.SetClip((new Rectangle(0, 0, RenderSize.X, RenderSize.Y)));
        e.Graphics.DrawImageUnscaled(renderBuffer, new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
    }

    #endregion

    #region Drawing

    public readonly Pen defaultPen = new(Color.Red, 1);
    public readonly SolidBrush defaultBrush = new(Color.DarkSlateGray);

    public void DrawLine(Pen pen, Point pos1, Point pos2) => Renderer.DrawLine(pen, pos1, pos2);

    public void DrawLine(Color color, Point pos1, Point pos2) => DrawLine(new Pen(color, 1), pos1, pos2);
    public void DrawLine(Point pos1, Point pos2) => DrawLine(defaultPen, pos1, pos2);


    public void DrawRect(Pen pen, Rectangle rect) => Renderer.DrawRectangle(pen, rect);
    public void DrawRect(Color color, Rectangle rect)
    {
        var pen = new Pen(color, 1);
        DrawRect(pen, rect);
    }
    public void DrawRect(Rectangle rect) => DrawRect(defaultPen, rect);

    public void DrawRectangle(Pen pen, Rectangle rect)
    {
        Renderer.FillRectangle(pen.Brush, rect);
    }
    public void DrawRectangle(Color color, Rectangle rect)
    {
        var pen = new Pen(color, 1);
        DrawRectangle(pen, rect);
    }
    public void DrawRectangle(Rectangle rect) => DrawRectangle(defaultBrush.Color, rect);
    public void DrawSprite(Sprite sprite, Point position)
    {
        Point size = new Point(sprite.GetSource().Width, sprite.GetSource().Height).VirtualizePixel(MainCamera);
        Renderer.DrawImage(sprite.GetSource(), position.X, position.Y, size.X, size.Y);
    }
   
    #endregion

    #region Input
    public void RegisterInput(InputHandler handler)
    {
        if(inputHandler != null) RemoveInput(inputHandler);
        inputHandler = handler;
        GameWindow.KeyDown += handler.KeyDown;
        GameWindow.KeyUp += handler.KeyUp;

        GameWindow.MouseMove += handler.MouseMoved;
        GameWindow.MouseDown += handler.MouseDown;
        GameWindow.MouseUp += handler.MouseUp;
        GameWindow.MouseDoubleClick += handler.MouseDoubleClicked;
        GameWindow.MouseEnter += handler.MouseEnter;
        GameWindow.MouseLeave += handler.MouseLeave;
    }
    public void RemoveInput(InputHandler handler)
    {
        inputHandler = null;
        GameWindow.KeyDown -= handler.KeyDown;
        GameWindow.KeyUp -= handler.KeyUp;
        GameWindow.MouseMove -= handler.MouseMoved;
        GameWindow.MouseDown -= handler.MouseDown;
        GameWindow.MouseUp -= handler.MouseUp;
        GameWindow.MouseDoubleClick -= handler.MouseDoubleClicked;
        GameWindow.MouseEnter -= handler.MouseEnter;
        GameWindow.MouseLeave -= handler.MouseLeave;
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
    /// Called when the game window is drawed and always before update. Perfect for drawing background.
    /// </summary>
    protected virtual void OnDraw() { }

    /// <summary>
    /// Called after OnDraw function is called. Perfect for drawing foreground.
    /// </summary>
    protected virtual void OnAfterDraw() { }

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
