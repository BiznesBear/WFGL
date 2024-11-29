using System;
using System.Diagnostics;
using WFGL.Utilities;
using Timer = System.Windows.Forms.Timer;

namespace WFGL.Core;

/// <summary>
/// Updates the game.
/// </summary>
public class Time 
{
    // consts
    public const int DEFALUT_INTERVAL = 1000 / DEFALUT_FPS;
    public const int DEFALUT_FPS = 100;

    public event Action? Update = null;
    public Timer Timer { get; } = new();

    // settings
    public double TimeScale { get; set; } = 1.0;
    public float FramesPerSecond => framesPerSecond;

    // delta time
    public double DeltaTime => deltaTime * TimeScale;
    public double UncaledDeltaTime => deltaTime;
    public float DeltaTimeF => (float)DeltaTime;
    public float UncaledDeltaTimeF => (float)UncaledDeltaTime;

    // privates

    private readonly Stopwatch frameStopwatch;
    private DateTime previousTime = DateTime.Now;
    private double deltaTime;
    private float framesPerSecond;
    private int frames;

    public Time()
    {
        Timer.Tick += Tick;

        frameStopwatch = new Stopwatch();
        frameStopwatch.Start();
        SetInterval();
    }

    private void Tick(object? sender, EventArgs e)
    {
        Update?.Invoke();
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
