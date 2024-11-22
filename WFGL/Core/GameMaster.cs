using System.Drawing.Drawing2D;
using WFGL.Input;
using WFGL.Objects;
using WFGL.Rendering;

namespace WFGL.Core;

public abstract partial class GameMaster 
{
    #region Main
    public GameWindow GameWindow { get; }
    public View MainView { get; set; }
    public Time TimeMaster { get; } = new();
    public LayerMaster LayerMaster { get; } = new();
    public Hierarchy MainHierarchy { get; }
    public Graphics Renderer { get; private set; }

    #endregion

    // defaluts
    public Pen DefaultPen = Pens.Red;
    public SolidBrush defaultBrush = new(Color.DarkSlateGray);

    // events 
    public event Action? WhenUpdate;
    public event Action? WhenDraw;


    #region Settings
    public bool IsFullScreen { get; private set; }
    public bool WindowAspectLock { get; set; } = false;
    public SmoothingMode Smoothing { get; set; } = SmoothingMode.None;
    public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

    #endregion

    #region Getters
    public InputHandler InputMaster => GameWindow.GetInput();
    public VirtualUnit VirtualScale => GameWindow.VirtualScale;
    public Size VirtualSize => VirtualScale.GetSize();

    #endregion

    // render view
    private Bitmap renderBuffer;
    private Size windowSizeTemp;

    public GameMaster(GameWindow window)
    {
        // IMPORTANT: DO NOT CHANGE THE ORDER
        GameWindow = window;
        windowSizeTemp = GameWindow.ClientSize;

        
        GameWindow.Paint += Draw;
        GameWindow.ResizeEnd += Resized;

        MainView = new(this, ViewOptions.Default);
        MainHierarchy = new Hierarchy(this);

        TimeMaster.Timer.Tick += Update;

        renderBuffer = new Bitmap(VirtualSize.Width, VirtualSize.Height);
        Renderer = Graphics.FromImage(renderBuffer);

        ResetRenderBuffer();

        WhenUpdate += OnUpdate;
        WhenDraw += OnDraw;

        WhenUpdate += MainHierarchy.OnUpdate;
        WhenDraw += MainHierarchy.OnDraw;
    }
    
    public void Load()
    {
        // IMPORTANT: DO NOT CHANGE THE ORDER
        TimeMaster.Start();
        OnLoad();
        GameWindow.ShowDialog();
    }
    private void Update(object? sender, EventArgs e)
    {
        GameWindow.Invalidate(new Rectangle(MainView.RealPosition.X, MainView.RealPosition.Y, VirtualSize.Width, VirtualSize.Height));
        WhenUpdate?.Invoke();
    }

    private void Draw(object? sender, PaintEventArgs e)
    {
        WhenDraw?.Invoke();
        OnAfterDraw();

        e.Graphics.SetClip((new Rectangle(MainView.RealPosition.X, MainView.RealPosition.Y, VirtualSize.Width, VirtualSize.Height)));
        e.Graphics.DrawImageUnscaled(GetRender(), new Rectangle(0, 0, VirtualSize.Width, VirtualSize.Height));
    }
    private void Resized(object? sender, EventArgs e)
    {
        if (WindowAspectLock) GameWindow.ClientSize = MainView.GetAspect();
        ResetRenderBuffer();
        OnResized();
        ResetRenderClip();
    }

    #region RenderBuffer
    public Bitmap GetRender() => renderBuffer;
    public void ResetRenderBuffer()
    {
        renderBuffer.Dispose();
        renderBuffer = new Bitmap(VirtualSize.Width, VirtualSize.Height);

        Renderer = Graphics.FromImage(renderBuffer);

        Renderer.SmoothingMode = Smoothing;
        Renderer.InterpolationMode = Interpolation;
        ResetRenderClip();
        OnRenderBufferReset();
    }
    public void ResetRenderClip() 
    {
        Renderer.SetClip((new Rectangle(MainView.RealPosition.X, MainView.RealPosition.Y, VirtualSize.Width, VirtualSize.Height)));
    }
    #endregion


    // full screen window
    public void FullScreen()
    {
        windowSizeTemp = GameWindow.ClientSize;
        GameWindow.FormBorderStyle = FormBorderStyle.None;
        GameWindow.WindowState = FormWindowState.Maximized;
        GameWindow.TopMost = true;
        IsFullScreen = true;
        ResetRenderBuffer();
        OnResized();
    }


    // make window normal again
    public void NormalScreen()
    {
        GameWindow.FormBorderStyle = GameWindow.borderStyle;
        GameWindow.WindowState = FormWindowState.Normal;
        GameWindow.TopMost = false;
        GameWindow.ClientSize = windowSizeTemp;
        IsFullScreen = false;
        ResetRenderBuffer();
        OnResized();
    }

    public void RegisterHierarchy(Hierarchy hierarchy) 
        => MainHierarchy.Register(hierarchy);
    public void DeregisterHierarchy(Hierarchy hierarchy)
        => MainHierarchy.Deregister(hierarchy);


    #region EventFunctions

    /// <summary>
    /// Called after showing window dialog. 
    /// </summary>
    protected virtual void OnLoad() { }

    /// <summary>
    /// Called every frame. 
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// Called when the game window is drawed and always before update. 
    /// </summary>
    protected virtual void OnDraw() { }

    /// <summary>
    /// Called after OnDraw and before update function is called. 
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
