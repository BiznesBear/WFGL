using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class Time 
{
    public const int DEFALUT_INTERVAL = 1000 / DEFALUT_FPS;
    public const int DEFALUT_FPS = 100;

    public Timer Timer { get; } = new();

    // Getters 
    public float DeltaTime => (float)deltaTime;
    public float Fps => framesPerSecond;



    private DateTime previousTime = DateTime.Now;
    private double deltaTime;
    private float framesPerSecond;

    private readonly Stopwatch frameStopwatch;
    private int frames;

    public Time()
    {
        Timer.Tick += Tick;

        frameStopwatch = new Stopwatch();
        frameStopwatch.Start();
        SetFps();
    }
    
    // TODO: Rework if possible 
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
