using WFGL.Core
    ;
namespace DemoGame;

internal class Program
{
    #pragma warning disable
    public static TestPlaceMaster testPlaceInstance;
    public static GravityTestsMaster gravityTestsInstance;
    public static OrtographicCubeMaster ortographicCubeGame;
    public static LabMaster labInstance;
    #pragma warning restore

    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default);

        //ortographicCubeGame = new(window);
        //ortographicCubeGame.Load();

        testPlaceInstance = new(window);
        testPlaceInstance.Load();

        //labInstance = new(window);
        //labInstance.Load();

        //gravityTestsInstance = new(window);
        //gravityTestsInstance.Load();
    }
}