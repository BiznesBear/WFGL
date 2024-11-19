using WFGL.Objects;

namespace WFGL.Core;
public class View : Transform
{
    public const uint DEFALUT_TARGET = 500; // defalut target for window is 500x500 pixels 

    public View(GameMaster m, ViewOptions options)
    {
        SetMaster(m);
        Target = options.Target;
    }


    /// <summary>
    /// Target aspect ratio of viewed render 
    /// </summary>
    public Size AspectRatio { get; set; }

    /// <summary>
    /// Target resolution of viewed render 
    /// </summary>
    public Size Resolution { get; set; } 

    /// <summary>
    /// Refrence size of rendering everything on window.
    /// </summary>
    public uint Target { get; set; } // refrence size for scaling 

    /// <summary>
    /// Used in scaling objects 
    /// </summary>
    public float Scaler => GetMaster().VirtualScale.FactorX / Target; // dividing virtual unit by refrence target to get real virtual unit 

    public float ViewDistance { get; set; } = 4;
    public float Fov { get; set; } = 500;

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
public struct ViewOptions(Size aspectRatio, Size resolution, uint target = View.DEFALUT_TARGET)
{
    public uint Target { get; set; } = target;
    public Size AspectRatio { get; set; } = aspectRatio;
    public Size Resolution { get; set; } = resolution;

    public readonly static ViewOptions Default = new(new Size(16, 9), new(700, 700));
}
