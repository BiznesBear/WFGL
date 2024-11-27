namespace WFGL;

/// <summary>
/// Debug tools.
/// </summary>
public static class WFGLSettings
{
    #region Wrint

    /// <summary>
    /// Enables info when new object is registered or deregistered.
    /// </summary>
    public static bool ShowRegisters { get; set; } = false;

    /// <summary>
    /// Enable Infos, Warrings and Errors.
    /// </summary>
    public static bool ShowEvents { get; set; } = false;

    /// <summary>
    /// Enable other custom wrints.
    /// </summary>
    public static bool ShowCustoms { get; set; } = true;

    #endregion

    /// <summary>
    /// Turn on/off all settings.
    /// </summary>
    public static bool All
    {
        set
        {
            ShowRegisters = value;
            ShowEvents = value;
            ShowCustoms = value;
        }
    }
}
