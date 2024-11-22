namespace WFGL.Input;

// TODO: Find a way to replace all "if(!Enabled) return;" with better system

/// <summary>
/// Handle input in more accessible way.
/// </summary>
public abstract class InputHandler
{
    public bool Enabled { get; set; } = true;
    public bool MouseInside { get; private set; }

    protected readonly HashSet<Keys> pressedKeys = new();
    internal void KeyDown(object? sender, KeyEventArgs e)
    {
        if(!Enabled) return;
        if (!pressedKeys.Contains(e.KeyCode)) OnKeyDown(e.KeyCode);
        pressedKeys.Add(e.KeyCode);
    }

    internal void KeyUp(object? sender, KeyEventArgs e)
    {
        if (!Enabled) return;

        pressedKeys.Remove(e.KeyCode);
        OnKeyUp(e.KeyCode);
    }

    internal void MouseMoved(object? sender, MouseEventArgs e)
    {
        if (!Enabled) return;
        Mouse.Position = e.Location;
    }
    internal void MouseDown(object? sender, MouseEventArgs e)
    {
        if (!Enabled) return;

        OnMouseDown(e.Button);
        Mouse.pressedButtons.Add(e.Button);
    }
    internal void MouseUp(object? sender, MouseEventArgs e)
    {
        if (!Enabled) return;

        OnMouseUp(e.Button);
        Mouse.pressedButtons.Remove(e.Button);
    }
    internal void MouseDoubleClicked(object? sender, MouseEventArgs e)
    {
        if (!Enabled) return;

        OnMouseDoubleClicked(e.Button);
    }
        
    internal void MouseEnter(object? sender, EventArgs e) { if (!Enabled) return; MouseInside = true; }
    internal void MouseLeave(object? sender, EventArgs e) { if (!Enabled) return; MouseInside = false; }
    internal void MouseWheel(object? sender, MouseEventArgs e) { OnMouseWheel(e.Delta); }

    public bool IsKeyPressed(Keys key) { if (!Enabled) return false; return pressedKeys.Contains(key); }

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
    protected virtual void OnMouseDown(MouseButtons buttons) { }

    /// <summary>
    /// Called when mouse is un clicked (any button)
    /// </summary>
    /// <param name="key">Last realesed key</param>
    protected virtual void OnMouseUp(MouseButtons buttons) { }

    /// <summary>
    /// Called when mouse is double clicked (any button)
    /// </summary>
    /// <param name="key">Last realesed key</param>
    protected virtual void OnMouseDoubleClicked(MouseButtons buttons) { }


    /// <summary>
    /// Called when mouse wheel moved.
    /// </summary>
    /// <param name="key">Last realesed key</param>
    protected virtual void OnMouseWheel(int delta) { }
}
