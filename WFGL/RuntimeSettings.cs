namespace WFGL;

/// <summary>
/// Debug tools.
/// </summary>
public static class RuntimeSettings
{
    /// <summary>
    /// Prints info to console when new object is registered or deregistered.
    /// </summary>
    public static bool ShowRegisters { get; set; } = false;

    /// <summary>
    /// Currently not applicable. 
    /// </summary>
    public static bool ShowEvents { get; set; } = false;

    /// <summary>
    /// Turn on/off all settings.
    /// </summary>
    public static bool All
    {
        set
        {
            ShowRegisters = value;
            ShowEvents = value;
        }
    }
}
