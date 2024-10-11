namespace WFGL.Input;

public static class Mouse
{
    public static Point Position { get; internal set; } = new Point();
    public static Rectangle Rect => new(Position.X-1, Position.Y-1, 3, 3);

    internal static HashSet<MouseButtons> pressedButtons = new();

    public static bool IsButtonPressed(MouseButtons button) => pressedButtons.Contains(button);

    /// <summary>
    /// Is mouse cursor inside window.
    /// </summary>
    public static bool Inside { get; internal set; }

    /// <summary>
    /// Total amount of mouse presses and releases.
    /// </summary>
    public static int Clicks { get; internal set; }

    public static bool IntersectsWithMouse(this Rectangle rect) => rect.IntersectsWith(Rect) && Inside;
}
