namespace WFGL.Input;

public static class Mouse
{
    public static Point Position { get; internal set; } = new Point();

    internal static HashSet<MouseButtons> pressedButtons = new();

    public static bool IsButtonPressed(MouseButtons button) => pressedButtons.Contains(button);

    /// <summary>
    /// Is mouse cursor inside window.
    /// </summary>
    public static bool Inside { get; internal set; }
    public static int Clicks { get; internal set; }

}
