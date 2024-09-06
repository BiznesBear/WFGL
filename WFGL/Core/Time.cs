using Timer = System.Windows.Forms.Timer;
namespace WFGL.Core;

public class Time 
{
    public const int DEFALUT_INTERVAL = 16;
    public const int DEFALUT_FPS = 60;
    public const int RECOMMENDED_FPS = 100;
    private static Time? Instance { get; set; }
    private static Time Get => Instance ?? throw new Exception("Time instance is null");
    public Time()
    {
        Instance = this;
        SetFps(DEFALUT_FPS);
    }
    internal Timer Timer { get; set; } = new() { Interval = DEFALUT_INTERVAL };

    public static void Start() => Get.Timer.Start();
    public static void Stop() => Get.Timer.Stop();
    public static void SetInterval(int ms = DEFALUT_INTERVAL) => Get.Timer.Interval = ms;
    public static void SetFps(int fps = DEFALUT_FPS) => Get.Timer.Interval = 1000 / fps;
}
