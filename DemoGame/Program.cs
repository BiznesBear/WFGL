using WFGL.Core;
using WFGL;
using System.Windows.Forms;
using System.Drawing;
namespace DemoGame;

internal class Program
{ 
    private static void Main(string[] args)
    {
        Game game = new(GameWindowOptions.Default with { Background = System.Drawing.Color.DarkSlateGray });
        game.CreateWindow();
    }
}

class Game(GameWindowOptions options) : GameMaster(options)
{
    Hierarchy hierarchy = new();


    Layer topLayer = new(255);

    Player? player;
    Sprite? sprite;



    protected override void OnLoad()
    {
        Time.SetFps(Time.RECOMMENDED_FPS);
        hierarchy.AssignMaster(this);
        Layer.Registration = [topLayer];

        player = new() { Layer = topLayer };
        sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\aushf.jpg") { SpriteSize = 0.5f };
        player.Create(hierarchy);
        sprite.Create(hierarchy);
    }
    protected override void OnUpdate()
    {
        hierarchy.UpdateAll();
    }
    protected override void OnDraw()
    {        
        hierarchy.DrawAll();
    }
    protected override void OnInput()
    {
        if (IsKeyPressed(Keys.F11))
        {
            if (!IsFullScreen) FullScreen();
            else NormalScreen();
        }
    }
    protected override void OnResize()
    {
        Debug.Info("Resized!");
    }
}
class Player : Transform
{
    internal Sprite sprite = new("C:\\Users\\ADAM\\Desktop\\CZYSCIEC\\furniture.png");
    int speed = 6;
    const int normalSpeed = 6;
    const int sprintSpeed = 10;
    public override void OnCreate()
    {
        sprite.SpriteSize = 0.3f;
        sprite.Size = new(0.8f,0.5f);
    }
    public override void OnUpdate(GameMaster m)
    {
        speed = m.IsKeyPressed(Keys.Space)? sprintSpeed : normalSpeed;

        Vector2 direction = Vector2.Zero;
        if (m.IsKeyPressed(Keys.A)) direction -= new Vector2(speed, 0f);
        if (m.IsKeyPressed(Keys.D)) direction += new Vector2(speed, 0f);
        if (m.IsKeyPressed(Keys.W)) direction -= new Vector2(0f, speed);
        if (m.IsKeyPressed(Keys.S)) direction += new Vector2(0f, speed);

        Position += direction.Normalize() * new Vector2(speed, speed);
        Position = Position.CutToBounds(sprite, m.GetWindow());
        sprite.Position = Position;
    }
    public override void OnDraw(GameMaster m)
    {
        m.DrawSprite(sprite);
    }
}
