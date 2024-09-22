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
}
public struct CameraOptions(Size aspectRatio,Size resolution,uint target = Camera.DEFALUT_TARGET)
{
    public Size AspectRatio { get; set; } = aspectRatio;
    public Size Resolution { get; set; } = resolution;
    public uint Target { get; set; } = target;

    //public readonly static CameraOptions Default = new(new Size(16, 9),new(1280, 720));
    public readonly static CameraOptions Default = new(new Size(16, 9),new(300, 300));
}