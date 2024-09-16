using WFGL;
using WFGL.Core;
using WFGL.UI;
using System.Windows.Forms;
using System.Drawing;
using WFGL.Input;

namespace DemoGame;

internal class Program
{ 
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default);
        Game game = new(window);
        game.Load();
    }
}

class Game : GameMaster
{
    Font font = new("Cascadia Mono Light", 12);

    Layer lightLayer = new(150);
    Layer topLayer = new(100);
    Layer underTopLayer = new(99);


    

    Hierarchy hierarchy;
    Hierarchy lights;
    Canvas canvas;

    Player player;
    internal SpriteRenderer sprite;
    internal SpriteRenderer sprite2;

    StaticText fpsText;
    StaticText userNameText;

    PseduoLight pseudoLight;


    public Game(GameWindow window) : base(window)
    {
        RegisterInput(new GameInput(this));
        Layer.Registration = [topLayer,underTopLayer,lightLayer];

        hierarchy = new(this);
        lights = new(this);
        canvas = new(this);
        fpsText = new(canvas, font, $"FPS: {TimeMaster.Fps}");
        userNameText = new(canvas, font, Environment.UserName);
        pseudoLight = new() { Position = 1.5f,Layer=lightLayer };

        player = new() { Layer = topLayer};
        sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\aushf.jpg") { Position = new(2.5f,0), Layer = underTopLayer};
        sprite2 = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\mozg-masz-wypadlo-ci.jpg") { Position = Vector2.One };
    }

    protected override void OnLoad()
    {
        player.Create(hierarchy);
        sprite.Create(hierarchy);
        sprite2.Create(hierarchy);
        pseudoLight.Create(lights);

        fpsText.Create(canvas);
        userNameText.Create(canvas);
    }


    protected override void OnUpdate()
    {
        hierarchy.UpdateAll();
        lights.UpdateAll();
        canvas.UpdateAll();

        userNameText.Position = player.Position + new Vector2(0.33f,-0.15f);
        fpsText.Content = $"FPS: {TimeMaster.Fps}";
       
        pseudoLight.Position = Mouse.Position.ToVector2(VirtualScale);
    }
    
    protected override void OnDraw()
    {
        // drawing background of virtual render size 
        Pixel scale = new((int)(VirtualScale.FactorX * VirtualUnit.SCALING), (int)(VirtualScale.FactorY * VirtualUnit.SCALING));

        DrawRectangle(Pixel.Zero, scale);
        DrawRect(WindowCenter-scale/new Pixel(2,2), scale);

        hierarchy.DrawAll();
        lights.DrawAll();
        canvas.DrawAll();

        // draws the rect of current render size
        //DrawRect(Pixel.Zero,new Pixel((int)(VirtualScaleX*VirtualUnit.SCALING), (int)(VirtualScaleY * VirtualUnit.SCALING)));
        //DrawRect(Pixel.Zero,new Pixel((int)(GetVUnitRatio().FactorX*VirtualUnit.SCALING), (int)(GetVUnitRatio().FactorY * VirtualUnit.SCALING)));
    }
}

class GameInput(GameMaster master) : InputHandler(master)
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            if (Master.IsFullScreen) Master.FullScreen();
            else Master.NormalScreen();
        }
    }
    protected override void OnKeyUp(Keys key)
    {
        
    }
}

class Player : Transform 
{
    static Sprite sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\furniture.png");
    internal SpriteRenderer playerSprite = new(sprite);

    float speed = normalSpeed;
    const float normalSpeed = 2.5f;
    const float sprintSpeed = 4;

    public override void OnCreate()
    {
        playerSprite.Sprite.Scale = new(1f,0.6f);
    }
    public override void OnUpdate(GameMaster m)
    {
        var input = m.InputMaster;
        speed = input.IsKeyPressed(Keys.Space)? sprintSpeed : normalSpeed;

        Vector2 direction = Vector2.Zero;
        if (input.IsKeyPressed(Keys.A)) direction -= new Vector2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vector2(speed, 0f);
        if (input.IsKeyPressed(Keys.W)) direction -= new Vector2(0f, speed);
        if (input.IsKeyPressed(Keys.S)) direction += new Vector2(0f, speed);

        Position += direction.Normalize() * speed * m.TimeMaster.DeltaTime;
        playerSprite.Position = Position;
    }
    public override void OnDraw(GameMaster m)
    {
        playerSprite.OnDraw(m);
        m.DrawRect(Position.ToPixel(m.VirtualScale), playerSprite.RealSize.VirtualizePixel(m.MainCamera));
    }
}
