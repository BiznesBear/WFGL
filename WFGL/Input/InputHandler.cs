namespace WFGL.Input;

public abstract class InputHandler(Core.GameMaster master)
{
    protected Core.GameMaster Master { get; } = master;
    protected readonly HashSet<Keys> pressedKeys = new();
    public bool enabled=true;
    internal void KeyDown(object? sender, KeyEventArgs e)
    {
        if(!enabled) return;
        if (!pressedKeys.Contains(e.KeyCode)) OnKeyDown(e.KeyCode);
        pressedKeys.Add(e.KeyCode);
    }

    internal void KeyUp(object? sender, KeyEventArgs e)
    {
        if (!enabled) return;

        pressedKeys.Remove(e.KeyCode);
        OnKeyUp(e.KeyCode);
    }

    public bool IsKeyPressed(Keys key) { if (!enabled) return false; return pressedKeys.Contains(key); }

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


    /// <summary>
    /// Called when mouse is clicked (any button)
    /// </summary>
    /// <param name="key">Last realesed key</param>
    public virtual void OnMouseDown(MouseButtons buttons) { }

    /// <summary>
    /// Called when mouse is un clicked (any button)
    /// </summary>
    /// <param name="key">Last realesed key</param>
    public virtual void OnMouseUp(MouseButtons buttons) { }

    /// <summary>
    /// Called when mouse is double clicked (any button)
    /// </summary>
    /// <param name="key">Last realesed key</param>
    public virtual void OnMouseDoubleClicked(MouseButtons buttons) { }
}
