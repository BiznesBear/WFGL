﻿using System.Drawing;
using System.Windows.Forms;

using WFGL;
using WFGL.Core;
using WFGL.Input;
using WFGL.Physics;
using WFGL.UI;

namespace DemoGame;

public class GravityTestsMaster : GameMaster
{
    // assets
    public readonly static Font font = new("Consolas", 12);
    public readonly static Sprite maszWypadloCi = new("mozg-masz-wypadlo-ci.jpg");
    public readonly static Sprite playerSprite = new("furniture.png");

    // layers
    public readonly static Layer canvasLayer = new(300);
    public readonly static Layer topLayer = new(100);
    public readonly static Layer underTopLayer = new(99);

    // hroups, groups & hierarchys
    internal Group<ICollide> colliders;

    internal Hierarchy objects;
    internal Hierarchy canvas;
    internal StaticRenderHroup background;

    // objects

    internal CollidingSprite sprite = new("aushf.jpg") { Position = new(0, 4f), Layer = underTopLayer };
    internal CollidingSprite sprite2 = new("aushf.jpg") { Position = new(3.5f, 4f), Layer = underTopLayer };
    internal RigidPlayer player;

    StringRenderer fpsText;
    StringRenderer userNameText = new(font, Environment.UserName);


    public GravityTestsMaster(GameWindow window) : base(window)
    {
        RegisterInput(new GravityTestsInput(this));
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
        background.Objects = [
            sprite,
            sprite2,
            new SpriteRenderer(maszWypadloCi) { Position = 1 },
            new SpriteRenderer(maszWypadloCi) { Position = 3 },
            new SpriteRenderer(maszWypadloCi) { Position = 2 },
            new SpriteRenderer(maszWypadloCi) { Position = 4 },
            new SpriteRenderer(maszWypadloCi) { Position = new(0, 4) },
            new SpriteRenderer(maszWypadloCi) { Position = new(0, 3) },
            new SpriteRenderer(maszWypadloCi) { Position = Vec2.Zero },
            new SpriteRenderer(maszWypadloCi) { Position = new(3, 0) },
        ];
        objects.Objects = [
            player,
            background,
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
        userNameText.Position = player.Position + new Vec2(0.33f, -0.15f);
        fpsText.Content = $"Velocity: {player.grav.Velocity}";

        var input = InputMaster;

        if (input.IsKeyPressed(Keys.Left)) MainCamera.Position -= new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Right)) MainCamera.Position += new Vec2(0.07f, 0f);
        if (input.IsKeyPressed(Keys.Up)) MainCamera.Position -= new Vec2(0f, 0.07f);
        if (input.IsKeyPressed(Keys.Down)) MainCamera.Position += new Vec2(0f, 0.07f);


        ResetRenderClip();
    }

    protected override void OnDraw()
    {
        // drawing background 
        DrawRectangle(new(MainCamera.RealPosition, RenderSize.PushToSize()));
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

internal class GravityTestsInput(GameMaster master) : InputHandler(master)
{
    protected override void OnKeyDown(Keys key)
    {
        if (key == Keys.F11)
        {
            // full screen
            if (!Master.IsFullScreen) Master.FullScreen();
            else Master.NormalScreen();
        }

        if(key == Keys.C)
        {
            Program.gravityTestsInstance.player.grav.AddForce(1, Vec2.Up);
        }

        if (key == Keys.P)
        {
            Master.MainCamera.Position = 0;
        }
    }
}

internal class RigidPlayer : Transform, ICollide
{
    // sub-objects
    internal SpriteRenderer playerSprite = new(GravityTestsMaster.playerSprite);

    // movement
    float speed = normalSpeed;
    const float normalSpeed = 2.5f;
    internal Vec2 dir;

    // physics
    public Vec2 ColliderSize => playerSprite.RealSize.VirtualizePixel(GetMaster().MainCamera).ToVec2(GetMaster());
    public Vec2 ColliderPosition => playerSprite.Position + dir;
    public Gravity grav = new();
    public RigidPlayer() { }
    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        playerSprite.Sprite.Scale = new(1f, 0.6f);
    }
    public override void OnUpdate(GameMaster m)
    {
        // movement
        Vec2 direction = Vec2.Zero;

        var input = m.InputMaster;

        if (input.IsKeyPressed(Keys.Space))
        {
            grav.AddForce(grav.Strenght * 2, Vec2.Up);
        }

        if (input.IsKeyPressed(Keys.A)) direction -= new Vec2(speed, 0f);
        if (input.IsKeyPressed(Keys.D)) direction += new Vec2(speed, 0f);

        if (this.IsColliding(Program.gravityTestsInstance.colliders, out ICollide? coll)) // to avoid player clipping
        {
            dir += Vec2.Up * grav.Strenght;
            grav.ResetVelocity();
        }
        else
        {
            Position = grav.Calculate(Position);
            dir = direction.Normalize() * speed * m.TimeMaster.DeltaTime;
        }

        Position += dir;

        playerSprite.Position = Position;
    }
    public override void OnDraw(GameMaster m)
    {
        // updating not registred object manually
        playerSprite.Draw(m, m.Renderer);
    }
}