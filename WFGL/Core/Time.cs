using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace WFGL.Core;


public class Time 
{
    // consts
    public const int DEFALUT_INTERVAL = 1000 / DEFALUT_FPS;
    public const int DEFALUT_FPS = 100;


    // settings
    public double TimeScale { get; set; } = 1.0;
    public Timer Timer { get; } = new();


    public float FramesPerSecond => framesPerSecond;
    public float DeltaTimeF => (float)(deltaTime * TimeScale);
    public double DeltaTime => deltaTime * TimeScale;

    public float UncaledDeltaTimeF => (float)deltaTime;
    public double UncaledTime => deltaTime;


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
        SetInterval();
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
    public void SetFps(int fps = DEFALUT_FPS) { if (fps > 0) Timer.Interval = DEFALUT_INTERVAL / fps; }

    public void AdvanceTime(double seconds)
    {
        deltaTime += seconds * TimeScale;
        frames++;
        previousTime = previousTime.AddSeconds(seconds);
    }
}
