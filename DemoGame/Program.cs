using WFGL;
using WFGL.Core;
using WFGL.UI;
using WFGL.Input;
using WFGL.Physics;

using System.Windows.Forms;
using System.Drawing;
namespace DemoGame;

internal class Program
{

    #pragma warning disable
    public static Game gameInstance;
    #pragma warning restore

    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default);
        gameInstance = new(window);
        gameInstance.Load();
    }
}

internal class Game : GameMaster
{
    // assets
    public readonly static Font font = new("Cascadia Mono Light", 12);
    public readonly static Sprite maszWypadloCi = new("mozg-masz-wypadlo-ci.jpg");
    public readonly static Sprite playerSprite = new("furniture.png");

    // layers
    readonly static Layer canvasLayer = new(300);
    readonly static Layer topLayer = new(100);
    readonly static Layer underTopLayer = new(99);

    // hroups, groups & hierarchys
    internal Group<ICollide> colliders;

    internal Hierarchy objects;
    internal Hierarchy canvas;
    internal StaticRenderHroup background;

    // objects

    internal CollidingSprite sprite = new ("aushf.jpg") { Position = new (2.5f,0), Layer = underTopLayer };
    internal Player player;

    StringRenderer fpsText;
    StringRenderer userNameText = new(font, Environment.UserName);

    RectangleButton myButton = new(Color.Wheat,Color.Blue,Color.Red) { Layer = canvasLayer };

    public Game(GameWindow window) : base(window)
    {
        RegisterInput(new GameInput(this));
        WindowAspectLock = true;

        LayerMaster.Layers = [topLayer,underTopLayer,canvasLayer];

        objects = new(this);
        canvas = new(this) { Layer = canvasLayer };
        fpsText = new(font, $"FPS: {TimeMaster.Fps}");
        player = new(this) { Layer = topLayer };
        background = new(this);
        colliders = new(background);
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
            new SpriteRenderer(maszWypadloCi) { Position = 1 },
            new SpriteRenderer(maszWypadloCi) { Position = 3 },
            new SpriteRenderer(maszWypadloCi) { Position = 2 },
            new SpriteRenderer(maszWypadloCi) { Position = 4 },
            new SpriteRenderer(maszWypadloCi) { Position = new(0, 4) },
            new SpriteRenderer(maszWypadloCi) { Position = new(0, 3) },
            new SpriteRenderer(maszWypadloCi){ Position = Vector2.Zero },
            new SpriteRenderer(maszWypadloCi) { Position = new(3, 0) },
        ];

        objects.Objects = [
            player,
            background,
            myButton
        ];

        canvas.Objects = [
            fpsText,
            userNameText,
        ];

        RegisterHierarchy(objects);
        RegisterHierarchy(canvas);

        colliders.Update();
        background.Render();
    }
    protected override void OnUpdate()
    {
        userNameText.Position = player.Position + new Vector2(0.33f,-0.15f);
        fpsText.Content = $"FPS: {TimeMaster.Fps}";
    }
    protected override void OnDraw()
    {
        // drawing background 
        DrawRectangle(new(new Point(), RenderSize.PushToSize()));
    }
    protected override void OnAfterDraw()
    {
        sprite.DrawColliderBounds(this);
        player.DrawColliderBounds(this);
    }

    protected override void OnResized()
    {
        // groups need to be rendered by hand 
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

internal class Player : Transform, ICollide 
{
    // sub-objects
    internal SpriteRenderer playerSprite = new(Game.playerSprite);

    // movement
    float speed = normalSpeed;
    const float normalSpeed = 2.5f;
    const float sprintSpeed = 4;
    internal Vector2 dir;


    // colliders 
    internal RayInfo hitInfo;
    public Vector2 ColliderSize => playerSprite.RealSize.VirtualizePixel(GetMaster().MainCamera).ToVector2(GetMaster().VirtualScale);
    public Vector2 ColliderPosition => playerSprite.Position + dir;

    private Vector2 inP; 

    public Player(GameMaster m) { master = m; }
    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        playerSprite.Sprite.Scale = new(1f,0.6f);
    }
    public override void OnUpdate(GameMaster m)
    {
        // movement
        var input = m.InputMaster;
        speed = input.IsKeyPressed(Keys.Space) ? sprintSpeed : normalSpeed;

        Vector2 direction = Vector2.Zero;
        if (input.IsKeyPressed(Keys.A)) direction -= new Vector2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vector2(speed, 0f);
        if (input.IsKeyPressed(Keys.W)) direction -= new Vector2(0f, speed);
        if (input.IsKeyPressed(Keys.S)) direction += new Vector2(0f, speed);

        dir = direction.Normalize() * speed * m.TimeMaster.DeltaTime;

        if (!this.IsColliding(Program.gameInstance.colliders, out ICollide? coll)) // to avoid player clipping
            Position += dir;
        playerSprite.Position = Position;

        
        // raycast
        Ray ray = new(Position, coll!=null? coll.ColliderPosition: 1, 2);
        ray.IsColliding(Program.gameInstance.sprite, out hitInfo);  
        inP = hitInfo.intersectionPoint;
    }
    public override void OnDraw(GameMaster m)
    {
        // updating not registred object manually
        playerSprite.OnDraw(m);

        // drawing ray gizmos
        hitInfo.ray.DrawGizmos(m, inP);
    }
}
