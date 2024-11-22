using WFGL.Core
    ;
namespace DemoGame;

internal class Program
{
    #pragma warning disable
    public static TestPlaceMaster testPlaceInstance;
    public static GravityTestsMaster gravityTestsInstance;
    public static OrtographicCubeMaster renderTestInstance;
    public static LabMaster labInstance;
    #pragma warning restore

    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default);

        //renderTestInstance = new(window);
        //renderTestInstance.Load();

        testPlaceInstance = new(window);
        testPlaceInstance.Load();

        //labInstance = new(window);
        //labInstance.Load();

        //gravityTestsInstance = new(window);
        //gravityTestsInstance.Load();
    }
}