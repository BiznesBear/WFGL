namespace WFGL.Core;

public class Canvas : Panel
{
    public Canvas()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.EnableNotifyMessage, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    }
}
