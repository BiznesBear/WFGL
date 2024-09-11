using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class Time 
{
    public const int DEFALUT_INTERVAL = 10;
    public const int DEFALUT_FPS = 100;

    internal DateTime previousTime = DateTime.Now;
    internal double deltaTime;
    internal float framesPerSecond;
    public static float DeltaTime => (float)GetInstance.deltaTime;
    public static float Fps => GetInstance.framesPerSecond;

    private static Time? Instance { get; set; }
    private static Time GetInstance => Instance ?? throw new Exception("Time instance is null");
    public Time()
    {
        Instance = this;
    }
    
    internal Timer Timer { get; set; } = new() { };

    public static void Start() => GetInstance.Timer.Start();
    public static void Stop() => GetInstance.Timer.Stop();
    public static void SetInterval(int ms = DEFALUT_INTERVAL) => GetInstance.Timer.Interval = ms;
    public static void SetFps(int fps = DEFALUT_FPS) => GetInstance.Timer.Interval = 1000 / fps;
}
