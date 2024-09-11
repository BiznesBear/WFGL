using WFGL;
using WFGL.Core;
using WFGL.UI;
using System.Windows.Forms;
using System.Drawing;

namespace DemoGame;

internal class Program
{ 
    private static void Main(string[] args)
    {
        GameWindow window = new GameWindow(GameWindowOptions.Default with { Background = Color.DarkSlateGray });
        Game game = new(window);
        game.CreateWindow();
    }
}

class Game : GameMaster
{
    Hierarchy hierarchy;

    Layer topLayer = new(255);
    Layer underTopLayer = new(254);

    Player player;
    internal SpriteRenderer sprite;
    internal SpriteRenderer sprite2;

    Canvas canvas;
    StaticText fpsText;
    StaticText userNameText;

    Font font = new("Cascadia Mono Light", 14);

    public Game(GameWindow window) : base(window)
    {
        Layer.Registration = [topLayer,underTopLayer];

        hierarchy = new(this);

        canvas = new(this);
        fpsText = new(canvas, font, $"FPS: {Time.Fps}");
        userNameText = new(canvas, font, Environment.UserName);

        player = new() { Layer = topLayer};
        sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\aushf.jpg") { Position = new(2.5f,0), Layer = underTopLayer};
        sprite2 = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\mozg-masz-wypadlo-ci.jpg") {  Position = Vector2.One };
    }

    protected override void OnLoad()
    {
        player.Create(hierarchy);
        sprite.Create(hierarchy);
        sprite2.Create(hierarchy);

        fpsText.Create(canvas);
        userNameText.Create(canvas);
    }


    protected override void OnUpdate()
    {
        hierarchy.UpdateAll();
        canvas.UpdateAll();
        userNameText.Position = player.Position + new Vector2(0.33f,-0.15f);
    }
    
    protected override void OnDraw()
    {
        fpsText.UpdateText($"FPS: {Time.Fps}");
        hierarchy.DrawAll();
        canvas.DrawAll();

        // currently draws the rect of render size
        DrawRect(Pixel.Zero,new Pixel((int)(VirtualScale.FactorX*VirtualUnit.SCALING), (int)(VirtualScale.FactorY * VirtualUnit.SCALING)));
    }

    protected override void OnInput()
    {
        if (IsKeyPressed(Keys.F11))
        {
            if (!IsFullScreen) FullScreen();
            else NormalScreen();
        }
        if (IsKeyPressed(Keys.V) && IsKeyPressed(Keys.C) )
        {
            Output.Info(Output.Command());
        }
    }
    protected override void OnResize()
    {

    }
}
class Player : Transform
{
    internal SpriteRenderer playerSprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\furniture.png");
    float speed = 4;
    const float normalSpeed =2.5f;
    const float sprintSpeed = 4;
    public override void OnCreate()
    {
        playerSprite.Sprite.Scale = new(1f,0.6f);
    }
    public override void OnUpdate(GameMaster m)
    {
        speed = m.IsKeyPressed(Keys.Space)? sprintSpeed : normalSpeed;

        Vector2 direction = Vector2.Zero;

        if (m.IsKeyPressed(Keys.A)) direction -= new Vector2(speed, 0f);
        if (m.IsKeyPressed(Keys.D)) direction += new Vector2(speed, 0f);
        if (m.IsKeyPressed(Keys.W)) direction -= new Vector2(0f, speed);
        if (m.IsKeyPressed(Keys.S)) direction += new Vector2(0f, speed);
       
        Position += direction.Normalize() * speed * Time.DeltaTime;
        playerSprite.Position = Position;
        
    }
    public override void OnDraw(GameMaster m)
    {
        m.DrawSprite(playerSprite, playerSprite.RealSize.VirtualizePixel(m.MainCamera));
        m.DrawRect(Position.ToPixel(m.VirtualScale), playerSprite.RealSize.VirtualizePixel(m.MainCamera));
    }
}
