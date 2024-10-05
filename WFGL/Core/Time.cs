using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class Time 
{
    public const int DEFALUT_INTERVAL = 1000 / DEFALUT_FPS;
    public const int DEFALUT_FPS = 100;

    internal Timer Timer { get; } = new();


    private static Time? Instance { get; set; }

    private DateTime previousTime = DateTime.Now;
    private double deltaTime;
    private float framesPerSecond;

    private Stopwatch frameStopwatch;
    private int frames;

    public float DeltaTime => (float)deltaTime;
    public float Fps => framesPerSecond;

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
            frames = 0;
            frameStopwatch.Restart();
        }
        if (frames > 0 || frameStopwatch.ElapsedMilliseconds > 0) 
            framesPerSecond = frames / (frameStopwatch.ElapsedMilliseconds / 1000f);
        DateTime currentTime = DateTime.Now;
        deltaTime = (currentTime - previousTime).TotalSeconds;
        previousTime = currentTime;
    }

    public void Start() => Timer.Start();
    public void Stop() => Timer.Stop();
    public void SetInterval(int ms = DEFALUT_INTERVAL) => Timer.Interval = ms;
    public void SetFps(int fps = DEFALUT_FPS) => Timer.Interval = 1000 / fps;
}
