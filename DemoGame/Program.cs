using WFGL;
using WFGL.Core;
using WFGL.UI;
using WFGL.Input;

using System.Windows.Forms;
using System.Drawing;

namespace DemoGame;

internal class Program
{
    [STAThread]
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

    

    Hierarchy objects;
    Hierarchy lights;
    Canvas canvas;

    Player player;

    Sprite maszWypadloCi = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\mozg-masz-wypadlo-ci.jpg");

    internal SpriteRenderer sprite;
    internal SpriteRenderer sprite2;
    internal SpriteRenderer sprite3;
    internal SpriteRenderer sprite4;
    internal SpriteRenderer sprite5;
    internal SpriteRenderer sprite6;
    internal SpriteRenderer sprite7;
    internal SpriteRenderer sprite8;
    internal SpriteRenderer sprite9;

    StringRenderer fpsText;
    StringRenderer userNameText;

    PseduoLight pseudoLight;


    public Game(GameWindow window) : base(window)
    {
        RegisterInput(new GameInput(this));
       
        Layer.Registration = [topLayer,underTopLayer,lightLayer];

        objects = new(this);
        lights = new(this);
        canvas = new(this);
        fpsText = new(canvas, font, $"FPS: {TimeMaster.Fps}");
        userNameText = new(canvas, font, Environment.UserName);
        pseudoLight = new() { Position = 1.5f,Layer=lightLayer };

        player = new() { Layer = topLayer};
        sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\aushf.jpg") { Position = new(2.5f,0), Layer = underTopLayer};
        sprite2 = new(maszWypadloCi) { Position = Vector2.One };
        sprite3 = new(maszWypadloCi) { Position = 3 };
        sprite4 = new(maszWypadloCi) { Position = 2 };
        sprite5 = new(maszWypadloCi) { Position = 4 };
        sprite6 = new(maszWypadloCi) { Position = new(0,4) };
        sprite7 = new(maszWypadloCi) { Position = Vector2.Zero };
        sprite8 = new(maszWypadloCi) { Position = new(0, 3) };
        sprite9 = new(maszWypadloCi) { Position = new(3, 0) };
    }

    protected override void OnLoad()
    {
        // don't create hierarchy like this
        //player.Create(objects);
        //sprite.Create(objects);
        //sprite2.Create(objects);
        //sprite3.Create(objects);
        //sprite4.Create(objects);
        //sprite5.Create(objects);
        //sprite6.Create(objects);
        //sprite7.Create(objects);
        //sprite8.Create(objects);
        //sprite9.Create(objects);
        // use this instead

        objects.Registration = [
            player,
            sprite,
            sprite2,
            sprite3,
            sprite4,
            sprite5,
            sprite6,
            sprite7,
            sprite8,
            sprite9,
        ];

        //pseudoLight.Create(lights);

        fpsText.Create(canvas);
        userNameText.Create(canvas);
    }

    protected override void OnUpdate()
    {
        //var playerPos = player.Position.ToPixel(VirtualScale);
        //var textPos = userNameText.Position.ToPixel(VirtualScale);
        //var text2Pos = fpsText.Position.ToPixel(VirtualScale);
        //GetWindow().Invalidate(new Rectangle(playerPos.X-3, playerPos.Y-3, player.playerSprite.RealSize.X+6, player.playerSprite.RealSize.Y+6));
        //GetWindow().Invalidate(new Rectangle(textPos.X, textPos.Y, player.playerSprite.RealSize.X, player.playerSprite.RealSize.Y));
        //GetWindow().Invalidate(new Rectangle(text2Pos.X, text2Pos.Y, 100, 100));


        userNameText.Position = player.Position + new Vector2(0.33f,-0.15f);
        fpsText.Content = $"FPS: {TimeMaster.Fps}";
        

        pseudoLight.Position = Mouse.Position.ToVector2(VirtualScale);
    }
    protected override void OnDraw()
    {
        // drawing background of virtual render size 
        Pixel scale = new(RenderSize.X, RenderSize.Y);
        DrawRectangle(Pixel.Zero, scale);

        
        // centered window view
        //DrawRect(WindowCenter-scale/new Pixel(2,2), scale);

        objects.DrawAll();
        lights.DrawAll();
        canvas.DrawAll();
    }
}

class GameInput(GameMaster master) : InputHandler(master)
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            if (!Master.IsFullScreen) Master.FullScreen();
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
        //m.DrawRect(Position.ToPixel(m.VirtualScale), playerSprite.RealSize.VirtualizePixel(m.MainCamera));
    }
}
