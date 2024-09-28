using WFGL.Core;
namespace WFGL;

public class Camera(GameMaster master,CameraOptions options) : Transform
{
    public const uint DEFALUT_TARGET = 500;
    private readonly GameMaster Master = master;
    /// <summary>
    /// Target aspect ratio of viewed render (for to do not resizing world space)
    /// </summary>
    public Size AspectRatio { get; set; } = options.AspectRatio;
    public Size Resolution { get; set; } = options.Resolution;
    public uint Target { get; set; } = options.Target; //TODO: Cut the render view for do not resizing world space 
    public VirtualUnit Scaler => new(Master.VirtualScale.FactorX / Target, Master.VirtualScale.FactorY / Target);


    public Size GetAspect()
    {
        var GameWindow = Master.GetWindow();

        var windowAspectRatio = (float)GameWindow.ClientSize.Width / GameWindow.ClientSize.Height;
        var targetAspectRatio = (float)AspectRatio.Width / AspectRatio.Height;

        // Dopasowanie obrazu do proporcji okna
        if (windowAspectRatio > targetAspectRatio)
        {
            // Zbyt szeroki, dopasuj wysokość
            var width = (int)(GameWindow.ClientSize.Height * targetAspectRatio);
            return new Size(width, GameWindow.ClientSize.Height);
        }
        else if(windowAspectRatio < targetAspectRatio)
        {
            // Zbyt wąski, dopasuj szerokość
            var height = (int)(GameWindow.ClientSize.Width / targetAspectRatio);
            return new Size(GameWindow.ClientSize.Width, height);
        }
        return GameWindow.ClientSize;
    }
}
public struct CameraOptions(Size aspectRatio,Size resolution,uint target = Camera.DEFALUT_TARGET)
{
    public Size AspectRatio { get; set; } = aspectRatio;
    public Size Resolution { get; set; } = resolution;
    public uint Target { get; set; } = target;

    //public readonly static CameraOptions Default = new(new Size(16, 9),new(1280, 720));
    public readonly static CameraOptions Default = new(new Size(16, 9),new(300, 300));
}