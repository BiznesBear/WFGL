using WFGL.Core;
namespace WFGL;

public class Camera(GameMaster master) : Transform
{
    private GameMaster Master = master;

    /// <summary>
    /// Target aspect ratio of viewed render (for to do not resizing world space)
    /// </summary>
    public Size AspectRatio = new(16, 9);
    public uint Target { get; set; } = 500; //TODO: Cut the render view for do not resizing world space 

    public VirtualUnit Scaler => new(Master.VirtualScale.FactorX / Target, Master.VirtualScale.FactorY / Target);
}
