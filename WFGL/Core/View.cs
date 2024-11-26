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
    /// Refrence size of rendering everything on window.
    /// </summary>
    public uint Target { get; set; } // refrence size for scaling 

    /// <summary>
    /// Used in scaling objects 
    /// </summary>
    public float Scaler => GetMaster().VirtualScale.FactorX / Target; // dividing virtual unit by refrence target to get real virtual unit 

    public Size GetAspect()
    {
        var win = GetMaster().GameWindow;
        var winAspectRatio = (float)win.ClientSize.Width / win.ClientSize.Height;
        var targetAspectRatio = (float)AspectRatio.Width / AspectRatio.Height;

        if (winAspectRatio > targetAspectRatio)
        {
            var width = (int)(win.ClientSize.Height * targetAspectRatio);
            return new Size(width, win.ClientSize.Height);
        }
        else if (winAspectRatio < targetAspectRatio)
        {
            var height = (int)(win.ClientSize.Width / targetAspectRatio);
            return new Size(win.ClientSize.Width, height);
        }
        return win.ClientSize;
    }
}
public struct ViewOptions(Size aspectRatio, uint target = View.DEFALUT_TARGET)
{
    public uint Target { get; set; } = target;
    public Size AspectRatio { get; set; } = aspectRatio;

    public readonly static ViewOptions Default = new(new Size(16, 9));
}
