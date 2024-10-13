namespace WFGL.Input;

public static class Mouse
{
    public static Point Position { get; internal set; } = new Point();
    public static Rectangle Rect => new(Position.X-1, Position.Y-1, 3, 3);

    internal static HashSet<MouseButtons> pressedButtons = new();

    public static bool IsButtonPressed(MouseButtons button) => pressedButtons.Contains(button);
    public static bool IntersectsWithMouse(this Rectangle rect, InputHandler handler) => rect.IntersectsWith(Rect) && handler.MouseInside;
}
