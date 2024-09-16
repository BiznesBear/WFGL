using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class Time 
{
    public const int DEFALUT_INTERVAL = 10;
    public const int DEFALUT_FPS = 100;

    internal Timer Timer { get; } = new();


    private static Time? Instance { get; set; }
    private static Time GetInstance => Instance ?? throw new Exception("Time instance is null");

    private DateTime previousTime = DateTime.Now;
    private double deltaTime;
    private float framesPerSecond;

    private Stopwatch frameStopwatch;
    private int frames;

    public float DeltaTime => (float)GetInstance.deltaTime;
    public float Fps => GetInstance.framesPerSecond;

    public Time()
    {
        Instance = this;
        Timer.Tick += Tick;

        frameStopwatch = new Stopwatch();
        frameStopwatch.Start();
    }
   
    private void Tick(object? sender, EventArgs e)
    {
        frames++;
        if (frameStopwatch.ElapsedMilliseconds >= 1000)
        {
            framesPerSecond = frames / (frameStopwatch.ElapsedMilliseconds / 1000f);
            frames = 0;
            frameStopwatch.Restart();
        }

        DateTime currentTime = DateTime.Now;
        deltaTime = (currentTime - previousTime).TotalSeconds;
        previousTime = currentTime;
    }

    public static void Start() => GetInstance.Timer.Start();
    public static void Stop() => GetInstance.Timer.Stop();
    public static void SetInterval(int ms = DEFALUT_INTERVAL) => GetInstance.Timer.Interval = ms;
    public static void SetFps(int fps = DEFALUT_FPS) => GetInstance.Timer.Interval = 1000 / fps;
}
