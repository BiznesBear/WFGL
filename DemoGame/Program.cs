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

        //ortographicCube = new(window);
        //ortographicCube.Load();

        testPlace = new(window);
        testPlace.Load();

        //labInstance = new(window);
        //labInstance.Load();

        //gravityTestsInstance = new(window);
        //gravityTestsInstance.Load();
    }
}