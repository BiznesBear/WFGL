using System.Drawing;
using System.Windows.Forms;

using WFGL;
using WFGL.Core;
using WFGL.Input;
using WFGL.Physics;
using WFGL.UI;
using WFGL.Rendering;
using WFGL.Objects;
using WFGL.Components;

namespace DemoGame;


public class TestPlaceMaster : GameMaster
{
    // assets
    public readonly static Font font = new("Cascadia Mono Light", 12);


    // layers
    public readonly static Layer canvasLayer = new(300);
    public readonly static Layer topLayer = new(100);
    public readonly static Layer underTopLayer = new(99);

    // hroups, groups & hierarchys
    internal Group<ICollide> colliders;

    internal Hierarchy objects;
    internal Hierarchy canvas;
    internal StaticDrawHierarchy background;

    // objects

    internal CollidingBitmapRenderer sprite = new("assets/aushf.jpg") { Position = new(2.5f, 0), Layer = underTopLayer };
    internal TestPlacePlayer player;

    StringRenderer fpsText;
    StringRenderer userNameText = new(font, Environment.UserName);

    TextRectButton myButton = new("__________",Color.Wheat, Color.Blue, Color.Red) { Layer = canvasLayer };


    public TestPlaceMaster(GameWindow window) : base(window)
    {
        WFGLSettings.All = true;
        GameWindow.RegisterInput(new TestPlaceInput());

        LayerMaster.Layers = [topLayer, underTopLayer, canvasLayer];

        objects = new(this);
        canvas = new(this) { Layer = canvasLayer };
        fpsText = new(font, $"FPS: {TimeMaster.FramesPerSecond}");
        player = new() { Layer = topLayer };
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
        
        background.Init = [
            sprite,
            new BitmapRenderer(Assets.maszWypadloCi) { Position = 1 },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = 3 },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = 2 },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = 4 },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = new(0, 4) },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = new(0, 3) },
            new BitmapRenderer(Assets.maszWypadloCi){ Position = Vec2.Zero },
            new BitmapRenderer(Assets.maszWypadloCi) { Position = new(3, 0) },
        ];

        objects.Init = [
            player,
            background,
            myButton
        ];

        canvas.Init = [
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
        userNameText.Position = player.Position + new Vec2(0.33f, -0.15f);
        fpsText.Content = $"FPS: {TimeMaster.FramesPerSecond}";


        var input = InputMaster;

        if (input.IsKeyPressed(Keys.Left)) MainView.Position -= new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Right)) MainView.Position += new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Up)) MainView.Position -= new Vec2(0f, 0.07f);
        if (input.IsKeyPressed(Keys.Down)) MainView.Position += new Vec2(0f, 0.07f);

        ResetRenderClip();
    }
    protected override void OnDraw()
    {
        // drawing background 
        Renderer.FillRectangle(defaultBrush, new(0, 0, VirtualSize.Width,VirtualSize.Height));
    }
    protected override void OnAfterDraw()
    {
        sprite.DrawColliderBounds(this);
        player.DrawColliderBounds(this);
    }

    protected override void OnResized()
    {
        background.Render();
    }
}

internal class TestPlaceInput : InputHandler
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            // full screen
            if (!Program.testPlace.IsFullScreen) Program.testPlace.FullScreen();
            else Program.testPlace.NormalScreen();
        }
        if (key == Keys.P)
        {
            Program.testPlace.MainView.Position = 0;
        }
        
    }
}

internal class TestPlacePlayer : Transform, ICollide
{
    const float normalSpeed = 2.5f;
    const float sprintSpeed = 4;


    // sub-objects
    internal BitmapRenderer playerSprite = new(Assets.player);
    internal Vec2 dir;
    internal RaycastInfo hitInfo;


    public Vec2 ColliderSize => playerSprite.RealSize.VirtualizePixel(GetMaster().MainView).ToVec2(GetMaster().VirtualScale);
    public Vec2 ColliderPosition => playerSprite.Position + dir;


    private Vec2 inP;
    private float speed = normalSpeed;

    public TestPlacePlayer() { }
    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        playerSprite.Scale = new(1f, 0.6f);
    }
    public override void OnUpdate()
    {
        // movement
        var input = GetMaster().InputMaster;
        speed = input.IsKeyPressed(Keys.Space) ? sprintSpeed : normalSpeed;
        
        Vec2 direction = Vec2.Zero;

        if (input.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.W)) direction -= new Vec2(0f, speed);
        if (input.IsKeyPressed(Keys.S)) direction += new Vec2(0f, speed);

        dir = direction.Normalize() * speed * GetMaster().TimeMaster.DeltaTimeF;
        
        if (!this.IsColliding(Program.testPlace.colliders, out ICollide? coll)) // to avoid player clipping
            Position += dir;

        playerSprite.Position = Position;

        // raycast
        Ray ray = new(Position, coll != null ? coll.ColliderPosition : 1);
        ray.IsColliding(Program.testPlace.sprite, out hitInfo);
        inP = hitInfo.collisionPoint;

        
    }
    public override void OnDraw()
    {
        // updating not registred object manually
        playerSprite.Draw(GetMaster(), GetMaster().Renderer);

        // drawing ray gizmos
        hitInfo.ray.DrawGizmos(GetMaster(), inP);
    }
}