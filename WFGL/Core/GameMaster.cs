using System.Drawing;
using System.Drawing.Drawing2D;

using WFGL.Input;
namespace WFGL.Core;
public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    public bool IsFullScreen { get; private set; }

    public VirtualUnit VirtualScale => new VirtualUnit(RealVirtualScaleX, RealVirtualScaleY).Normalize();
    protected float RealVirtualScaleX => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Width);
    protected float RealVirtualScaleY => VirtualUnit.VirtualizeToFactor(GameWindow.ClientSize.Height);

    public Pixel WindowCenter => new(GameWindow.ClientSize.Width/2, GameWindow.ClientSize.Height/2);
    public Pixel WindowSize => new(GameWindow.ClientSize.Width, GameWindow.ClientSize.Height);
    public Pixel RenderSize => new((int)VirtualScale.FactorX * VirtualUnit.SCALING, (int)VirtualScale.FactorX * VirtualUnit.SCALING);

    // input

    public Camera MainCamera { get; private set; }

    public Time TimeMaster { get; } = new();
    public Graphics Renderer { get; private set; }

    private InputHandler? inputHandler;
    public InputHandler InputMaster => inputHandler ?? throw new WFGLError("Cannot use unregistered input handler");

    private Size windowSizeTemp;

    // events 
    public event WFGLEventArgs? WhenUpdate;
    public event WFGLEventArgs? WhenDraw;

    public Bitmap renderBuffer;

    public GameMaster(GameWindow window)
    {
        // window
        GameWindow = window;
        windowSizeTemp = GameWindow.ClientSize;


        // window events
        GameWindow.Paint += Draw;
        GameWindow.Resize += Resized;

        GameWindow.MouseMove += MouseMoved;
        MainCamera = new(this, CameraOptions.Default);



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
        Renderer.SmoothingMode = SmoothingMode.None;
        Renderer.InterpolationMode = InterpolationMode.NearestNeighbor;
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
        ResetRenderBuffer();
        OnResize();  
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
        e.Graphics.DrawImageUnscaled(renderBuffer, new Rectangle(0, 0, RenderSize.X, RenderSize.Y));
    }

    #endregion

    #region Drawing

    private readonly Pen defaultPen = new(Color.Red, 1);
    private readonly SolidBrush defaultBrush = new(Color.DarkSlateGray);

    public void DrawRect(Color color,float width, Pixel position, Pixel size)
    {
        Renderer.DrawRectangle(defaultPen, new(position.PushToPoint(), size.PushToSize()));
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

    public void DrawPseudoDarkness(int alpha=140)
    {
        DrawRectangle(Color.FromArgb(alpha, 0, 0, 0),Pixel.Zero,WindowSize); 
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

    #region OverwriteFunctions

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
