namespace WFGL.Input;

public abstract class InputHandler(Core.GameMaster master)
{
    protected Core.GameMaster Master { get; } = master;
    protected readonly HashSet<Keys> pressedKeys = new();

    internal void KeyDown(object? sender, KeyEventArgs e)
    {
        if (!pressedKeys.Contains(e.KeyCode)) OnKeyDown(e.KeyCode);
        pressedKeys.Add(e.KeyCode);
    }

    internal void KeyUp(object? sender, KeyEventArgs e)
    {
        pressedKeys.Remove(e.KeyCode);
        OnKeyUp(e.KeyCode);
    }

    public bool IsKeyPressed(Keys key) { return pressedKeys.Contains(key); }

    /// <summary>
    /// Called when any pressed down any key
    /// </summary>
    /// <param name="key">Last pressed down key</param>
    protected virtual void OnKeyDown(Keys key) { }

    /// <summary>
    /// Called when any released any key
    /// </summary>
    /// <param name="key">Last realesed key</param>
    protected virtual void OnKeyUp(Keys key) { }
}
