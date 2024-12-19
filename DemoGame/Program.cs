using System.Drawing;
using WFGL.Core;
namespace DemoGame;

internal class Program
{

    #pragma warning disable CS8618 
    public static TestPlaceMaster testPlace;
    public static GravityTestsMaster gravityTests;
    public static OrtographicCubeMaster ortographicCube;
    public static LabMaster lab;
    #pragma warning restore CS8618 


    [STAThread]
    private static void Main(string[] args)
    {
        GameWindow window = new(GameWindowOptions.Default with { Background = Color.Black });

        testPlace = new(window);
        testPlace.Load();

        //ortographicCube = new(window);
        //ortographicCube.Load();

        //gravityTests = new(window);
        //gravityTests.Load();

        //lab = new(window);
        //lab.Load();
    }
}