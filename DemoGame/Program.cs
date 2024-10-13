using WFGL.Core;
namespace DemoGame;

internal class Program
{

    #pragma warning disable
    public static TestPlaceMaster testPlaceInstance;
    public static RenderTestMaster renderTestInstance;
    #pragma warning restore

    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default);
        //renderTestInstance = new(window);
        //renderTestInstance.Load();
        testPlaceInstance = new(window);
        testPlaceInstance.Load();
    }
}