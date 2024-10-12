namespace WFGL.Core;

public class Camera : Transform3D
{
    public Camera(GameMaster m, CameraOptions options)
    {
        master = m;
        AspectRatio = options.AspectRatio;
        Resolution= options.Resolution;
        Target = options.Target;
    }


    public const uint DEFALUT_TARGET = 500;

    /// <summary>
    /// Target aspect ratio of viewed render (for to do not resizing world space)
    /// </summary>
    public Size AspectRatio { get; set; } 
    public Size Resolution { get; set; } 

    /// <summary>
    /// Refrence size of rendering everything on window.
    /// </summary>
    public uint Target { get; set; } 
    public float Scaler => GetMaster().VirtualScale.FactorX / Target;





    public Size GetAspect()
    {
        var GameWindow = GetMaster().GetWindow();

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