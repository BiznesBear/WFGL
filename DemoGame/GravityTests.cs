using System.Drawing;
using System.Windows.Forms;

using WFGL;
using WFGL.Components;
using WFGL.Core;
using WFGL.Input;
using WFGL.Objects;
using WFGL.Pseudo.Physics;
using WFGL.Rendering;
using WFGL.UI;

namespace DemoGame;

public class GravityTestsMaster : GameMaster
{
    // assets
    public readonly static Font font = new("Consolas", 12);

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

    internal CollidingBitmapRenderer sprite = new(Assets.aushf) { Position = new(0, 4f), Layer = underTopLayer };
    internal CollidingBitmapRenderer sprite2 = new(Assets.aushf) { Position = new(3.5f, 4f), Layer = underTopLayer };
    internal RigidPlayer player;

    StringRenderer fpsText;
    StringRenderer userNameText = new(font, Environment.UserName);


    public GravityTestsMaster(GameWindow window) : base(window)
    {
        GameWindow.RegisterInput(new GravityTestsInput());
        WindowAspectLock = true;

        LayerMaster.Layers = [topLayer, underTopLayer, canvasLayer];

        objects = new(this);
        canvas = new(this) { Layer = canvasLayer };
        fpsText = new(font, "");
        player = new() { Layer = topLayer };
        background = new(this);
        colliders = new(background);
    }

    protected override void OnLoad()
    {
        background.Init = [
            sprite,
            sprite2,
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
        ];

        canvas.Init = [
            fpsText,
            userNameText,
        ];

        RegisterHierarchy(objects);
        RegisterHierarchy(canvas);
        colliders.UpdateHashSet();
        background.Render();
    }
    protected override void OnUpdate()
    {
        userNameText.Position = player.Position + new Vec2(0.33f, -0.15f);
        fpsText.Content = $"Velocity: {player.Velocity}";

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
        Renderer.FillRectangle(defaultBrush, new(0, 0, VirtualSize.Width, VirtualSize.Height));
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

internal class GravityTestsInput : InputHandler
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            // full screen
            if (!Program.gravityTests.IsFullScreen) Program.gravityTests.FullScreen();
            else Program.gravityTests.NormalScreen();
        }

        if(key == Keys.C)
        {
            Program.gravityTests.player.ResetVelocity();
            Program.gravityTests.player.AddForce(new(300, Vec2.Up));
        }

        if (key == Keys.P)
        {
            Program.gravityTests.MainView.Position = 0;
        }
    }
}

internal class RigidPlayer : GravityTransform, ICollide
{
    // sub-objects
    internal BitmapRenderer playerSprite = new(Assets.player);

    // movement
    float speed = normalSpeed;
    const float normalSpeed = 2.5f;
    internal Vec2 dir;

    // physics
    public Vec2 ColliderSize => playerSprite.RealSize.VirtualizePixel(Master.MainView).ToVec2(Master.VirtualScale);
    public Vec2 ColliderPosition => playerSprite.Position + dir;

    public RigidPlayer() { }
    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        playerSprite.Scale = new(1f, 0.6f);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        // movement
        Vec2 direction = Vec2.Zero;

        var input = Master.InputMaster;

        if (input.IsKeyPressed(Keys.Space))
        {
            AddForce(new(1000f, Vec2.Up));
        }

        if (input.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);
        

        if (this.IsColliding(Program.gravityTests.colliders, out ICollide? coll)) // to avoid player clipping
        {
            dir += Vec2.Up * 0.01f;
            ResetVelocity();
        }
        else
        {
            dir = direction.Normalize() * speed * Master.TimeMaster.DeltaTimeF;
        }

        Position += dir;

        playerSprite.Position = Position;
    }
    public override void OnDraw()
    {
        // updating not registred object manually
        playerSprite.Draw(Master, Master.Renderer);
    }
}
