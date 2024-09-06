using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class GameMaster
{
    private GameWindow GameWindow { get; set; }

    public bool IsFullScreen { get; private set; }
    private Size windowSizeTemp;

    private Time TimeMaster { get; } = new();
    private Timer Timer { get; } = new();

    // input
    private readonly HashSet<Keys> pressedKeys = new();

    private Graphics Renderer { get; set; }


    

    public GameMaster(GameWindowOptions options)
    {
        // window
        Application.EnableVisualStyles();
        GameWindow = new();
        GameWindow.SetWindowOptions(options);
        GameWindow.Paint += Draw;

        GameWindow.Resize += Resized;

        // input
        GameWindow.KeyDown += KeyDown;
        GameWindow.KeyUp += KeyUp;

        // time
        TimeMaster.Timer = Timer;
        TimeMaster.Timer.Tick += Update;

        Renderer = GameWindow.CreateGraphics();
    }
    #region Window
    public void CreateWindow()
    {
        Time.Start();
        OnLoad();
        
        GameWindow.ShowDialog();
    }
    public GameWindow GetWindow() => GameWindow;

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
        GameWindow.FormBorderStyle = FormBorderStyle.FixedDialog;
        GameWindow.WindowState = FormWindowState.Normal;
        GameWindow.TopMost = false;
        GameWindow.ClientSize = windowSizeTemp;
        IsFullScreen = false;
    }
    private void Resized(object? sender, EventArgs e) { OnResize();  }
    #endregion

    #region Logic
    private void Update(object? sender, EventArgs e)
    {
        // update all game lib stuff here
        GameWindow.Invalidate();
        OnUpdate();
    }
    private void Draw(object? sender, PaintEventArgs e) 
    {
        // idk, but this is when window is updated, by user
        Renderer = e.Graphics;
        OnDraw();

        // use somthing like this for rotating objects 
        //Renderer.TranslateTransform(squarePosition.X + squareSize / 2, squarePosition.Y + squareSize / 2);
        //Renderer.RotateTransform(rotationAngle);
        // Renderer.ResetTransform();
    }
    #endregion

    #region Drawing
    public void DrawSprite(Sprite sprite)
    {
        Renderer.DrawImage(sprite.Source, sprite.Position.X, sprite.Position.Y, sprite.RealSize.X,sprite.RealSize.Y);
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
