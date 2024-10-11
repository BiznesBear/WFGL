namespace WFGL.Core;

public class Camera(GameMaster master, CameraOptions options) : Transform3D
{
    public const uint DEFALUT_TARGET = 500;
    private readonly GameMaster Master = master;

    /// <summary>
    /// Target aspect ratio of viewed render (for to do not resizing world space)
    /// </summary>
    public Size AspectRatio { get; set; } = options.AspectRatio;
    public Size Resolution { get; set; } = options.Resolution;

    /// <summary>
    /// Refrence size of rendering everything on window.
    /// </summary>
    public uint Target { get; set; } = options.Target;
    public float Scaler => Master.VirtualScale.FactorX / Target;

    public Size GetAspect()
    {
        var GameWindow = Master.GetWindow();

        var windowAspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
        var targetAspectRatio = (float)AspectRatio.Width / AspectRatio.Height;

        if (windowAspectRatio > targetAspectRatio)
        {
            var width = (int)(GameWindow.ClientSize.Height * targetAspectRatio);
            return new Size(width, GameWindow.ClientSize.Height);
        }
        else if (windowAspectRatio < targetAspectRatio)
        {
            var height = (int)(GameWindow.ClientSize.Width / targetAspectRatio);
            return new Size(GameWindow.ClientSize.Width, height);
        }
        return GameWindow.ClientSize;
    }
}
public struct CameraOptions(Size aspectRatio, Size resolution, uint target = Camera.DEFALUT_TARGET)
{
    public Size AspectRatio { get; set; } = aspectRatio;
    public Size Resolution { get; set; } = resolution;
    public uint Target { get; set; } = target;

    public readonly static CameraOptions Default = new(new Size(16, 9), new(300, 300));
}