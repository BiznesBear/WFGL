using WFGL;
using WFGL.Core;
using WFGL.UI;
using System.Drawing;
namespace DemoGame;

internal class Program
{

    #pragma warning disable
    public static TestPlaceMaster testPlaceInstance;
    #pragma warning restore

    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default with { Title = "WFGL test luncher"});
        testPlaceInstance = new(window);
        testPlaceInstance.Load();
    }
}
class Luncher : GameMaster
{
    private Hierarchy buttons;
    private LuncherButton testPlaceBtn;
    public Luncher(GameWindow window) : base(window)
    {
        RegisterInput(new LuncherInput(this));
       
        buttons = new(this);
        buttons.Objects = [
            testPlaceBtn = new LuncherButton("Test place") { Position = new(1.5f, 0.5f), Layer = Layer.Defalut }
        ];

        RegisterHierarchy(buttons);

        testPlaceBtn.OnClick += LoadTestPlace; 
    }
    private void LoadTestPlace() {
        GameWindow window = new(GameWindowOptions.Default with { Title = "TestPlace"});
        Program.testPlaceInstance = new(window);
    }
    protected override void OnDraw()
    {
        base.OnDraw();
        DrawRectangle(Color.DarkGray, RenderSize.PushToRect());
    }
}
class LuncherInput(GameMaster mase) : WFGL.Input.InputHandler(mase)
{

}
class LuncherButton : RectangleButton
{
    static Font defalutFont = new Font("Consolas",12);
    private StringRenderer text;

    public LuncherButton(string t) : base(Color.Black, Color.Green, Color.Brown)
    {
        text = new(defalutFont, t, Color.White);  
    }
    public override void OnUpdate(GameMaster m)
    {
        base.OnUpdate(m);
        text.Position = Position + RealRectSize.ToVector2(GetMaster().VirtualScale)/3 - new Vector2(0.2f,0);
        text.OnUpdate(m);
    }
    public override void OnDraw(GameMaster m)
    {
        base.OnDraw(m);
        text.OnDraw(m);
    }
}