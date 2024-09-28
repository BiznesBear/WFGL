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

internal class Game : GameMaster
{
    readonly static Font font = new("Cascadia Mono Light", 12);

    readonly static Layer lightLayer = new(150);
    readonly static Layer topLayer = new(100);
    readonly static Layer underTopLayer = new(99);


    Hierarchy objects;
    Hierarchy lights;
    Hierarchy canvas;

    Player player = new() { Layer = topLayer };

    readonly static Sprite maszWypadloCi = new("mozg-masz-wypadlo-ci.jpg");

    SpriteRenderer sprite = new ("aushf.jpg") { Position = new (2.5f,0), Layer = underTopLayer };
    SpriteRenderer sprite2 = new (maszWypadloCi) { Position = Vector2.One };
    SpriteRenderer sprite3 = new (maszWypadloCi) { Position = 3 };
    SpriteRenderer sprite4 = new(maszWypadloCi) { Position = 2 };
    SpriteRenderer sprite5 = new(maszWypadloCi) { Position = 4 };
    SpriteRenderer sprite6 = new(maszWypadloCi) { Position = new(0, 4) };
    SpriteRenderer sprite7 = new(maszWypadloCi) { Position = Vector2.Zero };
    SpriteRenderer sprite8 = new(maszWypadloCi) { Position = new(0, 3) };
    SpriteRenderer sprite9 = new(maszWypadloCi) { Position = new(3, 0) };

    StringRenderer fpsText;
    StringRenderer userNameText = new(font, Environment.UserName);

    DrawableGroup background;

    public Game(GameWindow window) : base(window)
    {
        RegisterInput(new GameInput(this));
        WindowAspectRatioLock = true;
        Layer.Layers = [topLayer,underTopLayer,lightLayer];

        objects = new(this);
        lights = new(this);
        canvas = new(this);
        fpsText = new(font, $"FPS: {TimeMaster.Fps}");

        background = new(this,objects);

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
        background.Objects = [
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
        background.Render();

        objects.Objects = [
            player,
            sprite,
            background
            //sprite2,
            //sprite3,
            //sprite4,
            //sprite5,
            //sprite6,
            //sprite7,
            //sprite8,
            //sprite9
        ];

        canvas.Objects = [
            fpsText,
            userNameText,
        ];
    }

    protected override void OnUpdate()
    {
        userNameText.Position = player.Position + new Vector2(0.33f,-0.15f);
        fpsText.Content = $"FPS: {TimeMaster.Fps}";
    }
    protected override void OnDraw()
    {
        // drawing background of virtual render size 
        DrawRectangle(Pixel.Zero.PushToPoint(), RenderSize);
        
        // centered window view
        //DrawRect(WindowCenter-scale/new Pixel(2,2), scale);

        objects.DrawAll();
        lights.DrawAll();
        canvas.DrawAll();
    }

    protected override void OnResized()
    {
        background.Render();
    }
}

internal class GameInput(GameMaster master) : InputHandler(master)
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            // full screen
            if (!Master.IsFullScreen) Master.FullScreen();
            else Master.NormalScreen();
        }
    }
}

internal class Player : Transform 
{
    static Sprite sprite = new("furniture.png");
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
    }
}
